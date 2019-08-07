using System;
using System.IO;
using System.Collections.Specialized;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;

using Egais.Entities.WB_DOC_SINGLE_01;

namespace Egais.Utm
{
    public class IUtmChannelFactory : ChannelFactory<IUtm>
    {
        public IUtmChannelFactory(string endpointConfigurationName) : base(endpointConfigurationName)
        {
            //Endpoint.Behaviors.Add(new UtmHttpBehavior());
            //Endpoint.Behaviors.Add(new UtmEndpointBehavior());
        }
    }

    public interface IUtmChannel : IUtm, IClientChannel
    {
    }

    public class UtmWebHttpBehavior : WebHttpBehavior
    {
        public UtmWebHttpBehavior() : base()
        {

        }
    }


    public partial class UtmClient 
    {
        #region UtmClient()
        public UtmClient(Uri serviceUri)
           : base(new WebHttpBinding(), new EndpointAddress(serviceUri))
        {
            //this.Endpoint.Behaviors.Add(new WebHttpBehavior());
            this.Endpoint.Behaviors.Add(new UtmWebHttpBehavior());
        }

        public UtmClient(WebHttpBinding binding, EndpointAddress address) : base(binding, address)
        {
            this.Endpoint.Behaviors.Add(new WebHttpBehavior());
        }

        public UtmClient(string endpointConfigurationName) : base(endpointConfigurationName)
        {
            //this.Endpoint.Behaviors.Add(new UtmEndpointBehavior());
        }

        public UtmClient(string endpointConfigurationName, string remoteAddress) : base(endpointConfigurationName, remoteAddress)
        {
        }

        public UtmClient(string endpointConfigurationName, EndpointAddress remoteAddress) : base(endpointConfigurationName, remoteAddress)
        {
        }

        public UtmClient(Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress)
        {
        }
        #endregion
    }

    public partial class UtmClient : IUtm
    {
        #region READ
        UtmResponse<Urls> IUtm.GetOut()
        {
            return Channel.GetOut();
        }

        UtmResponse<Urls> IUtm.GetIn()
        {
            return Channel.GetIn();
        }

        UtmResponse<Urls> IUtm.GetOutByReplyId(string replyId)
        {
            UtmResponse<Urls> response = null;
            using (OperationContextScope scope = new OperationContextScope(this.InnerChannel))
            {
                response = Channel.GetOutByReplyId(replyId);
            }
            return response;
        }

        UtmResponse<Total> IUtm.GetOutTotal()
        {
            return Channel.GetOutTotal();
        }

        UtmResponse<Urls> IUtm.GetOutDocumentsBodyTypePath(string path)
        {
            return Channel.GetOutDocumentsBodyTypePath(path);
        }

        Egais.Entities.WB_DOC_SINGLE_01.Documents IUtm.GetDocumentByPath(string path)
        {
            return Channel.GetDocumentByPath(path);
        }

        Stream IUtm.GetDocumentAsStreamByPath(string path)
        {
            return Channel.GetDocumentAsStreamByPath(path);
        }
        #endregion GET
        #region CREATE
        UtmResponse<Urls> IUtm.UploadFormData(Stream formData)
        {
            UtmResponse<Urls> response = null;
            if (formData is FormData
                    && WebOperationContext.Current != null)
            {
                WebOperationContext.Current.OutgoingRequest.Headers["Content-Type"] =
                    string.Format("multipart/form-data; boundary={0}", ((FormData)formData).Boundary);
            }
            response = this.Channel.UploadFormData(formData);

            return response;
        }

        UtmResponse<Urls> IUtm.UploadDocument(Egais.Entities.WB_DOC_SINGLE_01.Documents doc, string path)
        {
            UtmResponse<Urls> response = null;
            using (OperationContextScope scope = new OperationContextScope(this.InnerChannel))
            {
                UriTemplate uriTemplate = new UriTemplate("opt/in/{path}");
                OperationContext.Current.OutgoingMessageHeaders.To =
                    uriTemplate.BindByName(this.Endpoint.Address.Uri,
                                            new NameValueCollection() {
                                                { "path", path }
                                            });

                using (var formData = new FormData(doc))
                {
                    response = ((IUtm)this).UploadFormData(formData);
                }
            }
            return response;
        }
        #endregion POST
        #region DELETE
        void IUtm.DeleteDocument(string path)
        {
            Channel.DeleteDocument(path);
        }
        #endregion DELETE
    }

    public partial class UtmClient : ClientBase<IUtm>
    {
        public UtmResponseUrls GetOut()
        {
            return new UtmResponseUrls(((IUtm)this).GetOut());
        }

        public UtmResponseUrls GetOutByReplyId(string replyId)
        {
            return new UtmResponseUrls(((IUtm)this).GetOutByReplyId(replyId));
        }

        public UtmResponseTotal GetOutTotal()
        {
            return new UtmResponseTotal(((IUtm)this).GetOutTotal());
        }        

        public UtmResponseUrls GetOutDocuments(Egais.Entities.WB_DOC_SINGLE_01.ItemChoiceType bodyType)
        {
            string path = BodyTypeRouter.Path(bodyType);

            return new UtmResponseUrls(((IUtm)this).GetOutDocumentsBodyTypePath(path));
        }

        private bool SaveToFile(string fileName, Uri uri)
        {
            bool ret = false;
            System.IO.StreamWriter streamWriter = null;
            System.IO.StreamReader streamReader = null;

            string path = uri.LocalPath;

            if (path.StartsWith("/"))
                path = path.Remove(0,1);
            try
            {
                FileInfo xmlFile = new FileInfo(fileName);
                streamReader = new StreamReader(((IUtm)this).GetDocumentAsStreamByPath(path));
                streamWriter = xmlFile.CreateText();
                streamWriter.Write(streamReader.ReadToEnd());
                streamWriter.Close();
                streamReader.Close();
                ret = true;
            }
            //catch (System.Exception e)
            //{
            //    return false;
            //}
            finally
            {
                if ((streamWriter != null))
                {
                    streamWriter.Dispose();
                }

                if ((streamReader != null))
                {
                    streamReader.Dispose();
                }
            }

            return ret;
        }
        
        public Egais.Entities.WB_DOC_SINGLE_01.Documents GetDocument(string path)
        {
            if (path.StartsWith("/"))
                path = path.Remove(0, 1);

            return ((IUtm)this).GetDocumentByPath(path);
        }

        public Egais.Entities.WB_DOC_SINGLE_01.Documents GetDocumentByUri(Uri uri)
        {
            string path = uri.LocalPath;

            if (path.StartsWith("/"))
                path = path.Remove(0, 1);

            return ((IUtm)this).GetDocumentByPath(path);
        }

        public UtmResponseUrls UploadDocument(Egais.Entities.WB_DOC_SINGLE_01.Documents doc)
        {
            UtmResponseUrls utmResponseUrls = null;
            utmResponseUrls = new UtmResponseUrls(((IUtm)this).UploadDocument(doc, BodyTypeRouter.Path(doc.Document.ItemElementName)));
            return utmResponseUrls;
        }

        public void DeleteDocument(string path)
        {
            ((IUtm)this).DeleteDocument(path);
        }

        public void DeleteDocumentByUri(Uri uri)
        {
            if (uri.LocalPath.StartsWith("/"))
                ((IUtm)this).DeleteDocument(uri.LocalPath.Remove(0,1));
            else
                ((IUtm)this).DeleteDocument("");
        }
    }    
}
