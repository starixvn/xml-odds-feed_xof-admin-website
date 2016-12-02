namespace Fanex.OddsFeed.WebService.Controllers
{
    using System.IO;
    using System.Web.Mvc;
    using System.Xml;
    using System.Xml.Serialization;
    using OddsFeed.Common.DTO;
    using ONEbook.UM.Models;
    using ONEbook.UM.Services;
    using ONEbook.UM.Services.Impl;
    using WebServiceLibrary.Criteria;
    using WebServiceLibrary.Helpers;
    using WebServiceLibrary.Repositories;

    public class BaseController : Controller
    {
        private readonly IMaintenanceService _maintenanceService;
        private readonly IPartnerRepository _partnerRepository;

        public BaseController()
        {
            _maintenanceService = new MaintenanceService();
            _partnerRepository = new PartnerRepository();
        }

        protected bool IsDownTime()
        {
            return _maintenanceService.IsUnderMaintenance();
        }

        protected UnderMaintenanceTime GetUndermaintenanceTime()
        {
            return _maintenanceService.GetUnderMaintenanceTime();
        }

        protected QueryResult<UserProfile> GetAuthenticatedUserProfile(CheckPermissionCriteria criteria)
        {
            return _partnerRepository.GetAuthenticatedUserProfile(criteria);
        }

        protected string GetClientIpAddress()
        {
            return RequestHelper.GetClientIpAddress();
        }
        protected string SerializeObject<T>(T data)
        {
            using (var stringWriter = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(stringWriter, GetXmlWriterSettings()))
            {
                var serializer = new XmlSerializer(data.GetType());
                serializer.Serialize(xmlWriter, data, GetXmlSerializerNamespaces());
                return stringWriter.ToString();
            }
        }

        private XmlWriterSettings GetXmlWriterSettings()
        {
            return new XmlWriterSettings
            {
                Indent = false,
                OmitXmlDeclaration = true,
                NewLineHandling = NewLineHandling.None
            };
        }

        private XmlSerializerNamespaces GetXmlSerializerNamespaces()
        {
            var xmlNameSpace = new XmlSerializerNamespaces();
            xmlNameSpace.Add(string.Empty, string.Empty);
            return xmlNameSpace;
        }
    }
}