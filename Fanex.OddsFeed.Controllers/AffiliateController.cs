namespace Fanex.OddsFeed.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using Fanex.OddsFeed.Core;
	using Fanex.OddsFeed.Core.Attribute;
	using Fanex.OddsFeed.Core.Context;
	using Fanex.OddsFeed.Criteria.Affiliate;
	using Fanex.OddsFeed.Criteria.Licensee;
	using Fanex.OddsFeed.Criteria.User;
	using Fanex.OddsFeed.Models;
	using Fanex.OddsFeed.Models.ValueObject;
	using Fanex.OddsFeed.Resources;
	using Fanex.OddsFeed.Services.Interface;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.Rendering;

	[IsAuthorize]
	public class AffiliateController : BaseController
	{
		private const string MajorGroup = "Major";
		private const string MinorGroup = "Minor";
		private const string URLPrefix = "http://";
		private const string DefaultController = "Affiliate";
		private const string OddsRegionSettingType = "OddsRegion";
		private const string OddsTypeSettingType = "OddsType";
		private const byte OutrightDelayTimesIndex = 2;

		private readonly IAffiliateService affiliateService;
		private readonly ILicenseeService licenseeService;

		public AffiliateController(ICurrentContext currentContext,
			ISessionService sessionService,
			IBaseService baseService,
			IAffiliateService affiliateService,
			ILicenseeService licenseeService,
			AppSettings appSettings) : base(currentContext, sessionService, baseService, appSettings)
		{
			this.affiliateService = affiliateService;
			this.licenseeService = licenseeService;
		}

		#region Add New

		[HttpGet]
		public async Task<IActionResult> Add()
		{
			var criteria = new InsertAffiliateCriteria();
			ViewBag.VirtualPath = GetVirtualPath();

			// Domain
			var domains = await baseService.GetDataAsync<Domain>(new GetDomainBySystemIDCriteria { SystemID = sessionService.SystemId });
			ViewData["slAssignedDomain"] = domains.Select(x => new SelectListItem { Value = x.DomainId.ToString(), Text = x.DomainName }).ToList();
			ViewData["slDomain"] = new List<SelectListItem>();

			// Profile
			var licenseeProfile = await baseService.GetDataEntityAsync<LicenseeProfile>(new GetLicenseeProfileByLicenseeIDCriteria { SystemID = sessionService.SystemId });
			if (licenseeProfile != null)
			{
				criteria.OddsRegion = licenseeProfile.OddsRegion;
				criteria.NumberOfIPs = licenseeProfile.NumberOfIPs;

				// Delay Times
				var delayTimes = licenseeProfile.OddsDelayTimes.Split(',');
				criteria.DeadballDelayTimes = delayTimes.Length > 0 ? delayTimes[0] : "0";
				criteria.LiveDelayTimes = delayTimes.Length > 1 ? delayTimes[1] : "0";
				criteria.OutrightDelayTimes = delayTimes.Length > OutrightDelayTimesIndex ? delayTimes[OutrightDelayTimesIndex] : "0";
				criteria.ResultDelayTimes = licenseeProfile.ResultDelayTimes;
			}

			if (IsAccountManagerRole())
			{
				ViewBag.HasAccountManagerPermission = true;
				await AddAffiliateWithAccountManagerRoleAsync(criteria, licenseeProfile);
			}
			else if (IsLicenseeAdminRole())
			{
				await AddAffiliateWithLicenseeAdminRoleAsync(licenseeProfile);
			}
			else
			{
				await AddAffiliateWithSubAccountRoleAsync();
			}

			return View(criteria);
		}

		[HttpPost]
		public async Task<IActionResult> AddNew([FromBody] InsertAffiliateCriteria criteria)
		{
			var result = new CommandResult();
			await CustomizeDataBeforeAddNewAsync(criteria);
			if (await IsValidListOfIPsAsync(criteria.IpsFrom, criteria.IpsTo))
			{
				result = await affiliateService.InsertAsync(criteria);
				if (result.IsSuccessful)
				{
					result.Message = ViewResources.ActionResult_AddAffiliateSuccess;
				}
			}
			else
			{
				result.Code = 1;
				result.Message = ViewResources.Error_InvalidInput;
			}

			return Json(result);
		}

		[HttpPost]
		public async Task<IActionResult> CheckExistAffiliate([FromBody] InsertAffiliateCriteria criteria)
		{
			var result = new CommandResult { Code = 0 };
			if (string.IsNullOrEmpty(criteria.UserName))
			{
				return Json(result);
			}

			var affiliates = await baseService.GetDataAsync<Affiliate>(new CheckExistAffiliateCriteria
			{
				UserName = criteria.UserName,
				PageIndex = 1,
				PageSize = 1,
				SystemId = sessionService.SystemId
			});
			if (affiliates != null && affiliates.Any())
			{
				result.Code = 1;
			}

			return Json(result);
		}

		#endregion Add New

		#region Edit

		[HttpGet]
		public async Task<IActionResult> EditAffiliateInfo(int id, string userName)
		{
			if (id == 0)
			{
				return RedirectToAction("List", DefaultController);
			}

			var criteria = new UpdateAffiliateCriteria
			{
				PartnerID = id,
				UserName = userName
			};
			var affiliates = await baseService.GetDataAsync<Affiliate>(new GetAffiliateByNameCriteria
			{
				SystemId = sessionService.SystemId,
				UserName = userName,
				PageIndex = 1,
				PageSize = 1
			});
			if (affiliates != null && affiliates.Any())
			{
				var affiliate = affiliates.First();
				ViewBag.CurrentUser = " of \"" + userName + "\"";
				criteria.Email = affiliate.ContactEmail;
				criteria.Website = affiliate.Site;
				criteria.Note = affiliate.Remark;
			}

			return View(criteria);
		}

		[HttpPost]
		public async Task<IActionResult> EditAffiliateInfo(UpdateAffiliateCriteria criteria)
		{
			var result = new CommandResult();
			if (ModelState.IsValid)
			{
				criteria.UpdatedBy = sessionService.UserId;
				criteria.Email ??= string.Empty;
				criteria.Website ??= string.Empty;
				criteria.Note ??= string.Empty;

				result = await baseService.ExecuteNonQueryAsync(criteria);
				if (result.IsSuccessful)
				{
					result.Message = ViewResources.ActionResult_UpdateAffiliateInfoSuccess;
				}
			}

			return Json(result);
		}

		[HttpGet]
		public async Task<IActionResult> EditSetting(int id, string name)
		{
			if (id == 0)
			{
				return RedirectToAction("List", DefaultController);
			}

			ViewBag.CurrentUser = " of \"" + name + "\"";
			ViewBag.AffId = id;

			var affiliateProfile = await baseService.GetDataEntityAsync<LicenseeProfile>(new GetAffiliateProfileByIDCriteria { PartnerID = id });
			if (affiliateProfile != null)
			{
				ViewBag.NumberOfIPs = affiliateProfile.NumberOfIPs;
			}

			// Domain
			var domainSelList = await baseService.GetDataAsync<Domain>(new GetDomainByAffiliateIDCriteria { PartnerID = id });
			ViewData["slAssignedDomain"] = domainSelList.Select(x => new SelectListItem { Value = x.DomainId.ToString(), Text = x.DomainName }).ToList();

			var domains = await baseService.GetDataAsync<Domain>(new GetDomainBySystemIDCriteria { SystemID = sessionService.SystemId });
			ViewData["slDomain"] = domains.Where(x => !domainSelList.Any(s => s.DomainId == x.DomainId))
				.Select(x => new SelectListItem { Value = x.DomainId.ToString(), Text = x.DomainName }).ToList();

			// IP
			ViewBag.IpList = await affiliateService.GetPartnerIpByAffIdAsync(id);

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> EditSetting(int affId, string domains, string listIPs)
		{
			var result = new CommandResult();
			var fromIPs = new StringBuilder();
			var toIPs = new StringBuilder();

			if (!string.IsNullOrEmpty(listIPs))
			{
				var arrIps = listIPs.Split(',');
				const byte Iterator = 2;
				for (int i = 0; i < arrIps.Length; i += Iterator)
				{
					fromIPs.Append(arrIps[i] + ",");
					toIPs.Append(arrIps[i + 1] + ",");
				}
			}

			if (!await IsValidListOfIPsAsync(fromIPs.ToString(), toIPs.ToString()))
			{
				result.Code = 1;
				result.Message = ViewResources.Error_InvalidInput;
			}
			else
			{
				var criteria = new UpdateSettingCriteria
				{
					PartnerID = affId,
					Domains = domains,
					IpsFrom = fromIPs.ToString(),
					IpsTo = toIPs.ToString()
				};

				result = await baseService.ExecuteNonQueryAsync(criteria);
				if (result.IsSuccessful)
				{
					ViewBag.Success = true;
					ViewBag.PartnerID = criteria.PartnerID;
					result.Message = ViewResources.ActionResult_UpdateAffiliateInfoSuccess;
				}
			}

			return Json(result);
		}

		[HttpPost]
		public async Task<string> EditIPSetting(int affId, string fromIP, string toIP, string fromIPOld, string toIPOld)
		{
			var criteria = new UpdateSettingIPCriteria
			{
				PartnerID = affId,
				IpFrom = fromIP,
				IpTo = toIP,
				IpFromOld = fromIPOld ?? "",
				IpToOld = toIPOld ?? ""
			};

			var result = await baseService.ExecuteNonQueryAsync(criteria);
			if (result != null && result.IsSuccessful)
			{
				ViewBag.Success = true;
				ViewBag.PartnerID = criteria.PartnerID;
				return ViewResources.SuccessfulMessage;
			}
			else
			{
				return result != null ? result.Message : ViewResources.InternalErrorMessage;
			}
		}

		[HttpPost]
		public async Task<IActionResult> DeleteIPRange_EditSetting_Partner(DeleteSettingIP_AffiliateCriteria criteria)
		{
			criteria.Creator = sessionService.UserId;
			criteria.IPFromOld = string.IsNullOrEmpty(criteria.IPFromOld) ? string.Empty : criteria.IPFromOld;
			criteria.IPToOld = string.IsNullOrEmpty(criteria.IPToOld) ? string.Empty : criteria.IPToOld;
			criteria.IP = currentContext.IP;

			CommandResult result = await baseService.ExecuteNonQueryAsync(criteria);
			if (result.IsSuccessful)
			{
				result.Message = ViewResources.ActionResult_Success_DeleteIPSetting;
			}

			return Json(result);
		}

		[HttpGet]
		public async Task<IActionResult> EditProfile(int id, string name)
		{
			if (id == 0)
			{
				return RedirectToAction("List", DefaultController);
			}

			var criteria = new UpdateProfileCriteria { PartnerID = id };
			ViewBag.CurrentUser = $" of \"{name}\"";

			// Profile
			var affiliateProfile = await baseService.GetDataEntityAsync<LicenseeProfile>(new GetAffiliateProfileByIDCriteria { PartnerID = id });
			if (affiliateProfile != null)
			{
				criteria.OddsType = affiliateProfile.OddsType;
				criteria.OddsRegion = affiliateProfile.OddsRegion;
				criteria.LanguageList = affiliateProfile.LanguageList;
				criteria.SportIDList = affiliateProfile.SportIdList;
				criteria.CountryCodeList = affiliateProfile.CountryCodeList;
				criteria.BetTypeIDList = affiliateProfile.BetTypeIdList;

				ViewBag.OddsType = affiliateProfile.OddsType.Split(',');
				ViewBag.LanguageList = affiliateProfile.LanguageList.Split(',');
				ViewBag.SportIdList = affiliateProfile.SportIdList.Split(',');
				ViewBag.CountryCodeList = affiliateProfile.CountryCodeList.Split(',');
				ViewBag.BetTypeIdList = affiliateProfile.BetTypeIdList.Split(',');
			}

			if (IsAccountManagerRole())
			{
				ViewBag.HasAccountManagerPermission = true;
				await GetAffiliateProfileWithAccountManageRoleAsync(criteria, affiliateProfile);
			}
			else if (IsLicenseeAdminRole())
			{
				await GetAffiliateProfileWithLicenseeAdminRoleAsync(criteria);
			}
			else
			{
				// Sub-Acc
				await GetAffiliateProfileWithSubAccountRoleAsync(criteria);
			}

			return View(criteria);
		}

		[HttpPost]
		public async Task<IActionResult> EditProfile([FromBody] UpdateProfileCriteria criteria)
		{
			var result = new CommandResult();
			if (ModelState.IsValid)
			{
				await CustomizeDataBeforeEditProfileAsync(criteria);

				result = await baseService.ExecuteNonQueryAsync(criteria);
				if (result.IsSuccessful)
				{
					result.Message = ViewResources.ActionResult_UpdateAffiliateInfoSuccess;
				}
			}

			return Json(result);
		}

		public async Task<string> EditActive(int affId, bool status)
		{
			var criteria = new UpdateActiveCriteria
			{
				PartnerID = affId,
				IsAllowed = status,
				Creator = sessionService.UserId
			};
			var result = await baseService.ExecuteNonQueryAsync(criteria);

			return result.IsSuccessful ? ViewResources.SuccessfulMessage : result.Message;
		}

		#endregion Edit

		#region Get

		[HttpGet]
		//[Compress]
		public async Task<IActionResult> List()
		{
			ViewBag.SystemName = sessionService.SystemName;
			ViewBag.IsActivateSystem = await licenseeService.GetActiveBySystemIdAsync(sessionService.SystemId);
			return View("~/Views/Affiliate/AffiliateList.cshtml");
		}

		//[Compress]
		public async Task<IActionResult> GetAffiliateList(GetAffiliateByNameCriteria criteria)
		{
			criteria.SystemId = sessionService.SystemId;
			criteria.UserName = (criteria.UserName == null || criteria.UserName == "Username") ? string.Empty : criteria.UserName.Trim();
			criteria.PageIndex++;
			var result = await baseService.GetDataGridAsync<Affiliate>(criteria);
			return Json(result);
		}

		public async Task<string> ListExistedAff(string username)
		{
			var criteria = new GetTop10AffiliateCriteria
			{
				SystemId = sessionService.SystemId,
				Username = username
			};
			var affiliates = await baseService.GetDataAsync<Affiliate>(criteria);
			return string.Join(",", affiliates.Select(x => x.Name));
		}

		public async Task<IActionResult> ListExistedAffJson(string username)
		{
			var criteria = new GetTop10AffiliateCriteria
			{
				SystemId = sessionService.SystemId,
				Username = username
			};
			var affiliates = await baseService.GetDataAsync<Affiliate>(criteria);
			return Json(affiliates.Select(x => new JSONSource { Value = x.Id.ToString(), Label = x.Name }).ToList());
		}

		public async Task<IActionResult> ListExistedDomainsJson(string domainName)
		{
			var criteria = new GetTop10DomainCriteria
			{
				SystemId = sessionService.SystemId,
				DomainName = domainName
			};
			var domains = await baseService.GetDataAsync<Domain>(criteria);
			return Json(domains.Select(x => new JSONSource { Value = x.DomainId.ToString(), Label = x.DomainName }).ToList());
		}

		[HttpGet]
		public IActionResult Requests(string curmonth = "")
		{
			ViewBag.SystemName = sessionService.SystemName;
			ViewBag.MinDate = new DateTimeOffset(DefaultMinDate, 1, 1, 0, 0, 0, TimeSpan.FromHours(-7)).DateTime;
			ViewBag.MaxDate = DateTimeOffset.Now.DateTime;
			if (curmonth != string.Empty)
			{
				var curDate = DateTimeOffset.Now.DateTime;
				ViewBag.StartDateConfig = new DateTimeOffset(curDate.Year, curDate.Month, 1, 0, 0, 0, TimeSpan.FromHours(-7)).DateTime;
			}

			var model = new RequestStatisticCriteria();
			model.Week = Utilities.GetIso8601WeekOfYear(model.DateStart).ToString();
			model.ToWeek = Utilities.GetIso8601WeekOfYear(model.DateEnd).ToString();

			return View("AffiliateRequest", model);
		}

		public async Task<IActionResult> GetPartnerRequestStatistic([FromBody] RequestStatisticCriteria criteria)
		{
			if (string.IsNullOrEmpty(criteria.PartnerName) || criteria.PartnerName.ToLowerInvariant() == ViewResources.Affiliate_Name.ToLowerInvariant())
			{
				criteria.PartnerName = string.Empty;
			}

			if (string.IsNullOrEmpty(criteria.DomainName) || criteria.DomainName.ToLowerInvariant() == ViewResources.Domain_Name.ToLowerInvariant())
			{
				criteria.DomainName = string.Empty;
			}

			criteria.SystemId = sessionService.SystemId;
			criteria.PageIndex++;

			var result = await baseService.GetDataGridAsync<LicenseeRequestDetailStatistic>(criteria);
			return Json(result);
		}

		[HttpGet]
		public async Task<IActionResult> ViewXMLURLs_Edit(int id, string name)
		{
			if (id == 0)
			{
				return RedirectToAction("List", DefaultController);
			}

			var affiliateProfile = await baseService.GetDataEntityAsync<LicenseeProfile>(new GetAffiliateProfileByIDCriteria { PartnerID = id });
			var sports = await baseService.GetDataAsync<Sport>(new GetSupportedSportCriteria());
			if (affiliateProfile.SportIdList.Equals("all"))
			{
				ViewBag.SportIds = sports.Select(item => item.SportType.ToString()).ToList();
			}
			else
			{
				ViewBag.SportIds = affiliateProfile.SportIdList.Split(',');
			}

			ViewBag.CurrentUser = name;
			ViewBag.AffiliateName = name;

			var domains = await baseService.GetDataAsync<Domain>(new GetDomainByAffiliateIDCriteria { PartnerID = id });
			ViewBag.AssignedDomain = domains.Select(x => new SelectListItem { Value = x.DomainId.ToString(), Text = x.DomainName }).ToList();
			return View();
		}

		public string GetXMLOddsFeedLink(string domainURL, string affiliateName)
		{
			return string.IsNullOrEmpty(domainURL)
				? string.Empty
				: URLPrefix + domainURL.Replace(URLPrefix, string.Empty) + "/" + GetVirtualPath() + affiliateName + "/odds.xml";
		}

		public string GetXMLOddsFeed_HelpLink(string domainURL, string affiliateName)
		{
			return string.IsNullOrEmpty(domainURL) ?
				string.Empty :
				URLPrefix + domainURL.Replace(URLPrefix, string.Empty) + "/" + GetVirtualPath() + affiliateName + "/help.htm";
		}

		#endregion Get

		#region Delete

		public async Task<string> Delete(int affId)
		{
			var criteria = new DeleteAffiliateCriteria
			{
				Creator = sessionService.UserId,
				PartnerID = affId
			};

			CommandResult result = await baseService.ExecuteNonQueryAsync(criteria);
			return result.IsSuccessful ? ViewResources.SuccessfulMessage : result.Message;
		}

		#endregion Delete

		#region Common

		private async Task<bool> IsValidListOfIPsAsync(string ipsFrom, string ipsTo)
		{
			uint totalIPs = 0;
			var arrIpsFrom = ipsFrom.Split(',');
			var arrIpsTo = ipsTo.Split(',');

			var config = await baseService.GetDataAsync<SystemSetting>(new GetTypeSettingCriteria { SystemID = 0, Type = "Config" });
			var maxNumOfIPs = int.Parse(config.First(m => m.Key == "MaximumNumberOfIPs").Value);

			for (int i = 0; i < arrIpsFrom.Length; i++)
			{
				var ipFrom = Utilities.ConvertIPToUint(arrIpsFrom[i]);
				var ipTo = Utilities.ConvertIPToUint(arrIpsTo[i]);

				if (ipTo > 0 && ipFrom > 0)
				{
					totalIPs += ipTo - ipFrom + 1;
				}
			}

			return totalIPs <= maxNumOfIPs;
		}

		private async Task CustomizeDataBeforeAddNewAsync(InsertAffiliateCriteria criteria)
		{
			var fromIPs = new StringBuilder();
			var toIPs = new StringBuilder();

			if (!string.IsNullOrEmpty(criteria.IpsFrom))
			{
				var arrIps = criteria.IpsFrom.Split(',');
				const byte Iterator = 2;
				for (int i = 0; i < arrIps.Length; i += Iterator)
				{
					fromIPs.Append(arrIps[i] + ",");
					toIPs.Append(arrIps[i + 1] + ",");
				}
			}

			criteria.IpsFrom = fromIPs.ToString();
			criteria.IpsTo = toIPs.ToString();
			await AddDataToInsertAffiliateCriteria(criteria);
		}

		private async Task AddDataToInsertAffiliateCriteria(InsertAffiliateCriteria criteria)
		{
			criteria.SystemID = sessionService.SystemId;
			criteria.Creator = sessionService.UserId;

			criteria.Ip = currentContext.IP;
			criteria.Content ??= string.Empty;
			criteria.Note ??= string.Empty;
			criteria.Domains ??= string.Empty;
			criteria.BetTypeIDList ??= string.Empty;
			criteria.LeagueIDList ??= string.Empty;
			criteria.OutRightIDList ??= string.Empty;
			criteria.LeagueKeyword ??= string.Empty;
			criteria.LeagueKeyword_O ??= string.Empty;

			criteria.SportIDList = UpdateSportIDListForCricket(criteria.SportIDList);
			criteria.LeagueIDList = await GetLeagueIdsAsync(criteria.SportIDList, criteria.CountryCodeList, criteria.LeagueIDList);
			criteria.OutRightIDList = await GetOutRightIdsAsync(criteria.SportIDList, criteria.CountryCodeList, criteria.OutRightIDList);
			criteria.SportIDList = await GetSportIdsAsync(criteria.SportIDList);
			criteria.CountryCodeList = await GetCountryCodesAsync(criteria.CountryCodeList);
			criteria.BetTypeIDList = await GetBetTypeIdsAsync(criteria.BetTypeIDList);
		}

		private async Task CustomizeDataBeforeEditProfileAsync(UpdateProfileCriteria criteria)
		{
			criteria.OddsType ??= string.Empty;
			criteria.OddsRegion ??= string.Empty;
			criteria.LanguageList ??= string.Empty;
			criteria.SportIDList ??= string.Empty;
			criteria.CountryCodeList ??= string.Empty;
			criteria.BetTypeIDList ??= string.Empty;

			criteria.Creator = sessionService.UserId;

			criteria.SportIDList = UpdateSportIDListForCricket(criteria.SportIDList);

			criteria.LeagueIDList = await GetLeagueIdsAsync(criteria.SportIDList, criteria.CountryCodeList, criteria.LeagueIDList);
			criteria.OutRightIDList = await GetOutRightIdsAsync(criteria.SportIDList, criteria.CountryCodeList, criteria.OutRightIDList);
			criteria.SportIDList = await GetSportIdsAsync(criteria.SportIDList);
			criteria.CountryCodeList = await GetCountryCodesAsync(criteria.CountryCodeList);
			criteria.BetTypeIDList = await GetBetTypeIdsAsync(criteria.BetTypeIDList);
		}

		private static string UpdateSportIDListForCricket(string sportIDList)
		{
			if (sportIDList == null)
			{
				return null;
			}

			var sportList = sportIDList.Split(',').ToList();

			if (sportList.Contains("27") && !sportList.Contains("50"))
			{
				sportList.Add("50");
			}

			if (!sportList.Contains("27") && sportList.Contains("50"))
			{
				sportList.Remove("50");
			}

			return string.Join(",", sportList.ToArray());
		}

		private async Task<string> GetLeagueIdsAsync(string sportIds, string countryCodes, string leagueIds)
		{
			if (!string.IsNullOrWhiteSpace(leagueIds))
			{
				var leagues = await baseService.GetDataAsync<League>(new GetLeagueCriteria
				{
					SportIDList = sportIds,
					CountryCodeList = countryCodes
				});
				var leagueList = leagues.Select(x => new SelectListItem { Value = x.LeagueId.ToString(), Text = x.LeagueName }).ToList();
				if (Utilities.IsExistInSelectListItem(leagueIds, leagueList))
				{
					return "all";
				}
			}

			return leagueIds;
		}

		private async Task<string> GetOutRightIdsAsync(string sportIds, string countryCodes, string outRightIds)
		{
			if (!string.IsNullOrWhiteSpace(outRightIds))
			{
				var outrights = await baseService.GetDataAsync<League>(new GetOutRightCriteria
				{
					SportIDList = sportIds,
					CountryCodeList = countryCodes
				});
				var outrightList = outrights.Select(x => new SelectListItem { Value = x.LeagueId.ToString(), Text = x.LeagueName }).ToList();
				if (Utilities.IsExistInSelectListItem(outRightIds, outrightList))
				{
					return "all";
				}
			}

			return outRightIds;
		}

		private async Task<string> GetSportIdsAsync(string sportIds)
		{
			if (!string.IsNullOrWhiteSpace(sportIds))
			{
				var sports = await baseService.GetDataAsync<Sport>(new GetSupportedSportCriteria());
				var sportList = sports.Select(x => new SelectListItem { Value = x.SportType.ToString(), Text = x.SportName }).ToList();
				if (Utilities.IsExistInSelectListItem(sportIds, sportList))
				{
					return "all";
				}
			}

			return sportIds;
		}

		private async Task<string> GetCountryCodesAsync(string countryCodes)
		{
			if (!string.IsNullOrWhiteSpace(countryCodes))
			{
				var countries = await baseService.GetDataAsync<Country>(new GetAllCountryCriteria());
				var countryList = countries.Select(x => new SelectListItem { Value = x.CountryCode, Text = x.CountryName }).ToList();
				if (Utilities.IsExistInSelectListItem(countryCodes, countryList))
				{
					return "all";
				}
			}

			return countryCodes;
		}

		private async Task<string> GetBetTypeIdsAsync(string betTypeIds)
		{
			if (!string.IsNullOrWhiteSpace(betTypeIds))
			{
				var bettypes = await baseService.GetDataAsync<BetType>(new GetSupportedBettypeCriteria());
				var bettypeList = bettypes.Select(x => new SelectListItem { Value = x.TypeId.ToString(), Text = x.TypeNameE }).ToList();
				if (Utilities.IsExistInSelectListItem(betTypeIds, bettypeList))
				{
					return "all";
				}
			}

			return betTypeIds;
		}

		private void ViewBagForFlags(
			bool oddsTypeFlag,
			bool oddsStyleFlag,
			bool languagesFlag,
			bool sportsFlag,
			bool countriesFlag,
			bool marketsFlag)
		{
			ViewBag.OddsTypeFlag = oddsTypeFlag;
			ViewBag.OddsStyleFlag = oddsStyleFlag;
			ViewBag.LanguagesFlag = languagesFlag;
			ViewBag.SportsFlag = sportsFlag;
			ViewBag.CountriesFlag = countriesFlag;
			ViewBag.MarketsFlag = marketsFlag;
		}

		#region Account Manager

		private async Task AddAffiliateWithAccountManagerRoleAsync(InsertAffiliateCriteria criteria, LicenseeProfile licenseeProfile = null)
		{
			if (licenseeProfile != null)
			{
				criteria.OddsType = licenseeProfile.OddsType;
				criteria.LanguageList = licenseeProfile.LanguageList;
				criteria.SportIDList = licenseeProfile.SportIdList;
				criteria.CountryCodeList = licenseeProfile.CountryCodeList;
				criteria.LeagueIDList = licenseeProfile.LeagueIdList;
				criteria.OutRightIDList = licenseeProfile.OutrightIdList;
				criteria.BetTypeIDList = licenseeProfile.BetTypeIdList;
				criteria.LeagueKeyword = licenseeProfile.LeagueKeyword;
				criteria.LeagueKeyword_O = licenseeProfile.LeagueKeyword_O;

				ViewBag.NumberOfIPs = licenseeProfile.NumberOfIPs;

				ViewBagForFlags(true, true, true, true, true, true);

				await ProfileForAddAffiliateWithAccountManagerRoleAsync(criteria);
			}
		}

		private async Task ProfileForAddAffiliateWithAccountManagerRoleAsync(InsertAffiliateCriteria criteria)
		{
			var oddsTypes = (await baseService.GetDataAsync<SystemSetting>(new GetTypeSettingCriteria { SystemID = 0, Type = OddsTypeSettingType }))
				.Where(x => x.Key != "Live" || (x.Key == "Live" && IsAccountManagerRole())).ToList();
			ViewBag.OddsTypeList = oddsTypes.Select(x => new SelectListItem { Value = x.Value, Text = x.Key }).ToList();

			var oddsStyles = await baseService.GetDataAsync<SystemSetting>(new GetTypeSettingCriteria { SystemID = 0, Type = OddsRegionSettingType });
			ViewBag.OddsStyleList = oddsStyles.Select(x => new SelectListItem { Value = x.Value, Text = x.Key }).ToList();

			var languages = await baseService.GetDataAsync<Language>(new GetAllLanguageCriteria());
			ViewBag.LanguageIDList = languages.Select(x => new SelectListItem { Value = x.Code, Text = x.Name_EN }).ToList();

			ViewBag.DelayTimeList = await GenerateDelayTimeAsync();
			ViewBag.ResultDelayTimeList = licenseeService.GetResultDelayTimeList();

			// Country
			var countries = await baseService.GetDataAsync<Country>(new GetAllCountryCriteria());
			ViewBag.CountriesList = countries.Select(x => new SelectListItem { Value = x.CountryCode, Text = x.CountryName }).ToList();
			if (criteria.CountryCodeList.Equals("all"))
			{
				criteria.CountryCodeList = string.Join(",", countries.Select(item => item.CountryCode));
			}

			// Sport
			var sports = UpdateSportListForCricket(await baseService.GetDataAsync<Sport>(new GetSupportedSportCriteria()));
			if (criteria.SportIDList.Equals("all"))
			{
				criteria.SportIDList = string.Join(",", sports.Select(item => item.SportType));
			}

			ViewBag.MajorSportList = sports.Where(sport => sport.Group == MajorGroup)
				.Select(x => new SelectListItem { Value = x.SportType.ToString(), Text = x.SportName }).ToList();
			ViewBag.MinorSportList = sports.Where(sport => sport.Group == MinorGroup)
				.Select(x => new SelectListItem { Value = x.SportType.ToString(), Text = x.SportName }).ToList();

			// Market
			var bettypes = await baseService.GetDataAsync<BetType>(new GetSupportedBettypeCriteria());
			if (criteria.BetTypeIDList.Equals("all"))
			{
				criteria.BetTypeIDList = string.Join(",", bettypes.Select(item => item.TypeId));
			}

			ViewBag.MajorBettypesList = bettypes.Where(x => x.Group == MajorGroup)
				.Select(x => new SelectListItem { Value = x.TypeId.ToString(), Text = x.TypeNameE }).ToList();
			ViewBag.MinorBettypesList = bettypes.Where(x => x.Group == MinorGroup)
				.Select(x => new SelectListItem { Value = x.TypeId.ToString(), Text = x.TypeNameE }).ToList();

			// League
			var leagues = await baseService.GetDataAsync<League>(new GetLeagueCriteria
			{
				SportIDList = criteria.SportIDList,
				CountryCodeList = criteria.CountryCodeList
			});
			ViewBag.LeagueList = leagues.Select(x => new SelectListItem { Value = x.LeagueId.ToString(), Text = x.LeagueName }).ToList();
			if (criteria.LeagueIDList.Equals("all"))
			{
				criteria.LeagueIDList = string.Join(",", leagues.Select(item => item.LeagueId));
			}

			// OutRight
			var outrights = await baseService.GetDataAsync<League>(new GetOutRightCriteria
			{
				SportIDList = criteria.SportIDList,
				CountryCodeList = criteria.CountryCodeList
			});
			ViewBag.OutrightList = outrights.Select(x => new SelectListItem { Value = x.LeagueId.ToString(), Text = x.LeagueName }).ToList();
			if (criteria.OutRightIDList.Equals("all"))
			{
				criteria.OutRightIDList = string.Join(",", outrights.Select(item => item.LeagueId));
			}
		}

		private async Task GetAffiliateProfileWithAccountManageRoleAsync(UpdateProfileCriteria criteria, LicenseeProfile affiliateProfile = null)
		{
			if (affiliateProfile != null)
			{
				// Delay Times
				var delayTimes = affiliateProfile.OddsDelayTimes.Split(',');
				criteria.DeadballDelayTimes = delayTimes.Length > 0 ? delayTimes[0] : "0";
				criteria.LiveDelayTimes = delayTimes.Length > 1 ? delayTimes[1] : "0";
				criteria.OutrightDelayTimes = delayTimes.Length > OutrightDelayTimesIndex ? delayTimes[OutrightDelayTimesIndex] : "0";
				criteria.ResultDelayTimes = affiliateProfile.ResultDelayTimes;

				criteria.NumberOfIPs = affiliateProfile.NumberOfIPs;
				criteria.LeagueKeyword = affiliateProfile.LeagueKeyword;
				criteria.LeagueKeyword_O = affiliateProfile.LeagueKeyword_O;
				criteria.LeagueIDList = affiliateProfile.LeagueIdList;
				criteria.OutRightIDList = affiliateProfile.OutrightIdList;
				ViewBag.ActualNumberOfIps = affiliateProfile.ActualNumberOfIPs;
			}

			var licenseeProfile = await baseService.GetDataEntityAsync<LicenseeProfile>(new GetLicenseeProfileByLicenseeIDCriteria { SystemID = sessionService.SystemId });
			if (licenseeProfile != null)
			{
				ViewBag.NumberOfIPs = licenseeProfile.NumberOfIPs;
				ViewBagForFlags(true, true, true, true, true, true);

				await ProfileForEditAffiliateWithAccountManagerRoleAsync(criteria);
			}
		}

		private async Task ProfileForEditAffiliateWithAccountManagerRoleAsync(UpdateProfileCriteria criteria)
		{
			var oddsTypes = await baseService.GetDataAsync<SystemSetting>(new GetTypeSettingCriteria { SystemID = 0, Type = OddsTypeSettingType });
			ViewBag.OddsTypeList = oddsTypes.Select(x => new SelectListItem { Value = x.Value, Text = x.Key }).ToList();

			var oddsStyles = await baseService.GetDataAsync<SystemSetting>(new GetTypeSettingCriteria { SystemID = 0, Type = OddsRegionSettingType });
			ViewBag.OddsStyleList = oddsStyles.Select(x => new SelectListItem { Value = x.Value, Text = x.Key }).ToList();

			var languages = await baseService.GetDataAsync<Language>(new GetAllLanguageCriteria());
			ViewBag.LanguageIDList = languages.Select(x => new SelectListItem { Value = x.Code, Text = x.Name_EN }).ToList();

			ViewBag.DelayTimeList = await GenerateDelayTimeAsync();
			ViewBag.ResultDelayTimeList = licenseeService.GetResultDelayTimeList();

			// Sport
			var sports = UpdateSportListForCricket(await baseService.GetDataAsync<Sport>(new GetSupportedSportCriteria()));
			if (criteria.SportIDList.Equals("all"))
			{
				criteria.SportIDList = string.Join(",", sports.Select(item => item.SportType));
			}

			ViewBag.MajorSportList = sports.Where(sport => sport.Group == MajorGroup)
				.Select(x => new SelectListItem { Value = x.SportType.ToString(), Text = x.SportName }).ToList();
			ViewBag.MinorSportList = sports.Where(sport => sport.Group == MinorGroup)
				.Select(x => new SelectListItem { Value = x.SportType.ToString(), Text = x.SportName }).ToList();

			// Market
			var bettypes = await baseService.GetDataAsync<BetType>(new GetSupportedBettypeCriteria());
			if (criteria.BetTypeIDList.Equals("all"))
			{
				criteria.BetTypeIDList = string.Join(",", bettypes.Select(item => item.TypeId));
			}

			ViewBag.MajorBettypesList = bettypes.Where(x => x.Group == MajorGroup)
				.Select(x => new SelectListItem { Value = x.TypeId.ToString(), Text = x.TypeNameE }).ToList();
			ViewBag.MinorBettypesList = bettypes.Where(x => x.Group == MinorGroup)
				.Select(x => new SelectListItem { Value = x.TypeId.ToString(), Text = x.TypeNameE }).ToList();

			// Country
			var countries = await baseService.GetDataAsync<Country>(new GetAllCountryCriteria());
			ViewBag.CountriesList = countries.Select(x => new SelectListItem { Value = x.CountryCode, Text = x.CountryName }).ToList();
			if (criteria.CountryCodeList.Equals("all"))
			{
				criteria.CountryCodeList = string.Join(",", countries.Select(item => item.CountryCode));
			}

			// OutRight
			var outrights = await baseService.GetDataAsync<League>(new GetOutRightCriteria
			{
				SportIDList = criteria.SportIDList,
				CountryCodeList = criteria.CountryCodeList
			});
			ViewBag.OutrightList = outrights.Select(x => new SelectListItem { Value = x.LeagueId.ToString(), Text = x.LeagueName }).ToList();
			if (criteria.OutRightIDList.Equals("all"))
			{
				criteria.OutRightIDList = string.Join(",", outrights.Select(item => item.LeagueId));
			}

			// League
			var leagues = await baseService.GetDataAsync<League>(new GetLeagueCriteria
			{
				SportIDList = criteria.SportIDList,
				CountryCodeList = criteria.CountryCodeList
			});
			ViewBag.LeagueList = leagues.Select(x => new SelectListItem { Value = x.LeagueId.ToString(), Text = x.LeagueName }).ToList();
			if (criteria.LeagueIDList.Equals("all"))
			{
				criteria.LeagueIDList = string.Join(",", leagues.Select(item => item.LeagueId));
			}
		}

		#endregion Account Manager

		#region LicenseeAdmin/Sub-Acc

		private async Task AddAffiliateWithLicenseeAdminRoleAsync(LicenseeProfile licenseeProfile = null)
		{
			if (licenseeProfile != null)
			{
				var oddStyles = licenseeProfile.OddsType;
				ViewBag.NumberOfIPs = licenseeProfile.NumberOfIPs;
				ViewBagForFlags(
					licenseeProfile.OddsTypeFlag,
					licenseeProfile.OddsRegionFlag,
					licenseeProfile.LanguageListFlag,
					licenseeProfile.SportIdListFlag,
					licenseeProfile.CountryCodeListFlag,
					licenseeProfile.BetTypeIdListFlag && (oddStyles != "8" && !string.IsNullOrWhiteSpace(oddStyles)));

				await ProfileForAddAffiliateWithLicenseeAdminAndSubAccountRoleAsync();
			}
		}

		private async Task GetAffiliateProfileWithLicenseeAdminRoleAsync(UpdateProfileCriteria criteria)
		{
			var licenseeProfile = await baseService.GetDataEntityAsync<LicenseeProfile>(new GetLicenseeProfileByLicenseeIDCriteria { SystemID = sessionService.SystemId });
			if (licenseeProfile != null)
			{
				var oddStyles = licenseeProfile.OddsType;
				ViewBag.NumberOfIPs = licenseeProfile.NumberOfIPs;
				ViewBag.OddsTypeFlag = licenseeProfile.OddsTypeFlag;
				ViewBag.OddsStyleFlag = licenseeProfile.OddsRegionFlag;
				ViewBag.LanguagesFlag = licenseeProfile.LanguageListFlag;
				ViewBag.SportsFlag = licenseeProfile.SportIdListFlag;
				ViewBag.CountriesFlag = licenseeProfile.CountryCodeListFlag;
				ViewBag.MarketsFlag = licenseeProfile.BetTypeIdListFlag && (oddStyles != "8" && !string.IsNullOrWhiteSpace(oddStyles));

				await ProfileForEditAffiliateWithLicenseeAdminAndSubAccountRoleAsync(criteria);
			}
		}

		private async Task AddAffiliateWithSubAccountRoleAsync()
		{
			var userProfile = await baseService.GetDataEntityAsync<PermissionFlag>(new UserGetPermissionCriteria
			{
				SystemID = sessionService.SystemId,
				UserID = sessionService.UserId
			});

			if (userProfile != null)
			{
				ViewBag.NumberOfIPs = userProfile.NumberOfIPs;
				ViewBagForFlags(
					userProfile.OddsTypeFlag,
					userProfile.OddsRegionFlag,
					userProfile.LanguageListFlag,
					userProfile.SportIDListFlag,
					userProfile.CountryCodeListFlag,
					userProfile.BetTypeIDListFlag);

				await ProfileForAddAffiliateWithLicenseeAdminAndSubAccountRoleAsync();
			}
		}

		private async Task GetAffiliateProfileWithSubAccountRoleAsync(UpdateProfileCriteria criteria)
		{
			var userProfile = await baseService.GetDataEntityAsync<PermissionFlag>(new UserGetPermissionCriteria
			{
				SystemID = sessionService.SystemId,
				UserID = sessionService.UserId
			});

			if (userProfile != null)
			{
				ViewBag.NumberOfIPs = userProfile.NumberOfIPs;
				ViewBag.OddsTypeFlag = userProfile.OddsTypeFlag;
				ViewBag.OddsStyleFlag = userProfile.OddsRegionFlag;
				ViewBag.LanguagesFlag = userProfile.LanguageListFlag;
				ViewBag.SportsFlag = userProfile.SportIDListFlag;
				ViewBag.CountriesFlag = userProfile.CountryCodeListFlag;
				ViewBag.MarketsFlag = userProfile.BetTypeIDListFlag;

				await ProfileForEditAffiliateWithLicenseeAdminAndSubAccountRoleAsync(criteria);
			}
		}

		private async Task ProfileForAddAffiliateWithLicenseeAdminAndSubAccountRoleAsync()
		{
			// OddsType
			var oddsTypes = (await baseService.GetDataAsync<SystemSetting>(new GetTypeSettingCriteria { SystemID = sessionService.SystemId, Type = OddsTypeSettingType }))
				.Where(item => item.Key != "Live" || (item.Key == "Live" && IsAccountManagerRole()));
			ViewBag.OddsTypeList = oddsTypes.Select(x => new SelectListItem { Value = x.Value, Text = x.Key }).ToList();

			// OddsStyle
			var oddsStyles = await baseService.GetDataAsync<SystemSetting>(new GetTypeSettingCriteria { SystemID = 0, Type = OddsRegionSettingType });
			ViewBag.OddsStyleList = oddsStyles.Select(x => new SelectListItem { Value = x.Value, Text = x.Key }).ToList();

			// Language
			var languages = await baseService.GetDataAsync<Language>(new GetLanguageBySystemIDCriteria { SystemID = sessionService.SystemId });
			ViewBag.LanguageIDList = languages.Select(x => new SelectListItem { Value = x.Code, Text = x.Name_EN }).ToList();

			// Market
			var bettypes = await baseService.GetDataAsync<BetType>(new GetBettypeBySystemIDCriteria { SystemID = sessionService.SystemId });
			ViewBag.MajorBetTypesList = bettypes.Where(x => x.Group == MajorGroup)
				.Select(x => new SelectListItem { Value = x.TypeId.ToString(), Text = x.TypeNameE }).ToList();
			ViewBag.MinorBetTypesList = bettypes.Where(x => x.Group == MinorGroup)
				.Select(x => new SelectListItem { Value = x.TypeId.ToString(), Text = x.TypeNameE }).ToList();

			// Country
			var countries = await baseService.GetDataAsync<Country>(new GetCountryBySystemIDCriteria { SystemID = sessionService.SystemId });
			ViewBag.CountriesList = countries.Select(x => new SelectListItem { Value = x.CountryCode, Text = x.CountryName }).ToList();

			// Sport
			var sports = UpdateSportListForCricket((await baseService.GetDataAsync<Sport>(new GetSportBySystemIDCriteria { SystemID = sessionService.SystemId })).OrderBy(s => s.SportName));
			ViewBag.MajorSportList = sports.Where(sport => sport.Group == MajorGroup)
				.Select(x => new SelectListItem { Value = x.SportType.ToString(), Text = x.SportName }).ToList();
			ViewBag.MinorSportList = sports.Where(sport => sport.Group == MinorGroup)
				.Select(x => new SelectListItem { Value = x.SportType.ToString(), Text = x.SportName }).ToList();
		}

		private async Task ProfileForEditAffiliateWithLicenseeAdminAndSubAccountRoleAsync(UpdateProfileCriteria criteria)
		{
			// OddsType
			var oddsTypes = (await baseService.GetDataAsync<SystemSetting>(new GetTypeSettingCriteria { SystemID = sessionService.SystemId, Type = OddsTypeSettingType }))
				.Where(item => item.Key != "Live" || (item.Key == "Live" && IsAccountManagerRole()));
			ViewBag.OddsTypeList = oddsTypes.Select(x => new SelectListItem { Value = x.Value, Text = x.Key }).ToList();

			// OddsStyle
			var oddsStyles = await baseService.GetDataAsync<SystemSetting>(new GetTypeSettingCriteria { SystemID = 0, Type = OddsRegionSettingType });
			ViewBag.OddsStyleList = oddsStyles.Select(x => new SelectListItem { Value = x.Value, Text = x.Key }).ToList();

			// Language
			var languages = await baseService.GetDataAsync<Language>(new GetLanguageBySystemIDCriteria { SystemID = sessionService.SystemId });
			ViewBag.LanguageIDList = languages.Select(x => new SelectListItem { Value = x.Code, Text = x.Name_EN }).ToList();

			// Sport
			var sports = UpdateSportListForCricket((await baseService.GetDataAsync<Sport>(new GetSportBySystemIDCriteria { SystemID = sessionService.SystemId })).OrderBy(s => s.SportName));
			if (criteria.SportIDList.Equals("all"))
			{
				criteria.SportIDList = string.Join(",", sports.Select(item => item.SportType));
			}
			ViewBag.MajorSportList = sports.Where(sport => sport.Group == MajorGroup)
				.Select(x => new SelectListItem { Value = x.SportType.ToString(), Text = x.SportName }).ToList();
			ViewBag.MinorSportList = sports.Where(sport => sport.Group == MinorGroup)
				.Select(x => new SelectListItem { Value = x.SportType.ToString(), Text = x.SportName }).ToList();

			// Market
			var bettypes = await baseService.GetDataAsync<BetType>(new GetBettypeBySystemIDCriteria { SystemID = sessionService.SystemId });
			if (criteria.BetTypeIDList.Equals("all"))
			{
				criteria.BetTypeIDList = string.Join(",", bettypes.Select(item => item.TypeId));
			}
			ViewBag.MajorBetTypesList = bettypes.Where(x => x.Group == MajorGroup)
				.Select(x => new SelectListItem { Value = x.TypeId.ToString(), Text = x.TypeNameE }).ToList();
			ViewBag.MinorBetTypesList = bettypes.Where(x => x.Group == MinorGroup)
				.Select(x => new SelectListItem { Value = x.TypeId.ToString(), Text = x.TypeNameE }).ToList();

			// Country
			var countries = await baseService.GetDataAsync<Country>(new GetCountryBySystemIDCriteria { SystemID = sessionService.SystemId });
			ViewBag.CountriesList = countries.Select(x => new SelectListItem { Value = x.CountryCode, Text = x.CountryName }).ToList();
			if (criteria.CountryCodeList.Equals("all"))
			{
				criteria.CountryCodeList = string.Join(",", countries.Select(item => item.CountryCode));
			}
		}

		private static IEnumerable<Sport> UpdateSportListForCricket(IEnumerable<Sport> sports)
		{
			const byte Cricket = 50;
			return sports.Where(s => s.SportType != Cricket).ToList();
		}

		#endregion LicenseeAdmin/Sub-Acc

		#endregion Common
	}
}
