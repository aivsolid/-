using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.IO;
using Egais.Entities;

namespace Egais.Utm
{
    [ServiceContract(Namespace = "")]
    public interface IUtm
    {
        [OperationContract]
        [WebGet(UriTemplate = "opt/in", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Xml)]
        UtmResponse<Urls> GetIn();

        [OperationContract]
        [WebGet(UriTemplate = "opt/out", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Xml)]
        UtmResponse<Urls> GetOut();

        [OperationContract]
        [WebGet(UriTemplate = "opt/out/total", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Xml)]
        UtmResponse<Total> GetOutTotal();

        [OperationContract]
        [WebGet(UriTemplate = "{path}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Xml)]
        [XmlSerializerFormat]
        Egais.Entities.WB_DOC_SINGLE_01.Documents GetDocumentByPath(string path);

        [OperationContract]
        [WebGet(UriTemplate = "{path}", BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetDocumentAsStreamByPath(string path);

        /*
        [OperationContract]
        [WebGet(UriTemplate = "out/replypartner", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Xml)]
        //[XmlSerializerFormat]
        //[UtmOperationBehavior]
        UtmResponse<Urls> GetReplyPartner();
        */

        [OperationContract]
        [WebGet(UriTemplate = "opt/out/{path}", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Xml)]
        UtmResponse<Urls> GetOutDocumentsBodyTypePath(string path);

        [OperationContract]
        [WebGet(UriTemplate = "opt/out?replyid={replyId}", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Xml)]
        UtmResponse<Urls> GetOutByReplyId(string replyId);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Xml, RequestFormat = WebMessageFormat.Xml)]
        UtmResponse<Urls> UploadFormData(Stream formData);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "opt/in/{path}", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Xml, RequestFormat = WebMessageFormat.Xml)]
        UtmResponse<Urls> UploadDocument(Egais.Entities.WB_DOC_SINGLE_01.Documents doc, string path);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "{path}", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Xml, RequestFormat = WebMessageFormat.Xml)]
        void DeleteDocument(string path);
    }    
}
