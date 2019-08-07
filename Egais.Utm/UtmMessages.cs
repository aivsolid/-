using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.IO;

namespace Egais.Utm
{
    public class UtmResponseUrls : UtmResponse<Urls>
    {
        public UtmResponseUrls():base()
        {
        }

        public UtmResponseUrls(UtmResponse<Urls> utmResponse) 
        {
            this.Content = utmResponse.Content;
            this.sign = utmResponse.sign;
            this.ver = utmResponse.ver;
        }

        public Url this[int index] { get => this.Content[index]; }
    }

    public class UtmResponseTotal : UtmResponse<Total>
    {
        public UtmResponseTotal() : base()
        {
        }

        public UtmResponseTotal(UtmResponse<Total> utmResponse)
        {
            this.Content = utmResponse.Content;
            this.sign = utmResponse.sign;
            this.ver = utmResponse.ver;
        }
    }
    public class UtmResponseError : UtmResponse<Error>
    {
        public UtmResponseError() : base()
        {
        }
        public UtmResponseError(UtmResponse<Error> utmResponse)
        {
            this.Content = utmResponse.Content;
            this.sign = utmResponse.sign;
            this.ver = utmResponse.ver;
        }

    }

    [XmlRoot(ElementName = "A", Namespace = "")]
    public class UtmResponse<T> : IXmlSerializable
    {
        public int ver;
        public string sign;
        private T content;

        public UtmResponse()
        {
            Content = default(T);
        }

        public T Content { get => content; set => content = value; }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            XmlSerializer xmlSerializer;
            reader.ReadStartElement();
            XmlNodeType nodeType = reader.MoveToContent();

            switch (typeof(T).Name)
            {
                case nameof(Total):
                case nameof(Error):
                    xmlSerializer = new XmlSerializer(typeof(T));
                    content = (T)xmlSerializer.Deserialize(reader);
                    break;
                case nameof(Urls):
                    xmlSerializer = new XmlSerializer(typeof(Url));
                    Content = (T)Activator.CreateInstance(typeof(T));
                    while (reader.NodeType == XmlNodeType.Element && reader.LocalName == "url")
                    {
                        ((IList<Url>)content).Add(item: (Url)xmlSerializer.Deserialize(reader));
                        reader.Skip();
                    }
                    break;
            }
            if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "sign")
            {
                reader.ReadStartElement();
                sign = reader.Value;
                reader.Skip();
            }
            if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "ver")
            {
                reader.ReadStartElement();
                ver = Int32.Parse(reader.Value);
                reader.Skip();
            }
            reader.ReadEndElement();
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            XmlSerializer xmlSerializer;

            switch (typeof(T).Name)
            {
                case nameof(Total):
                case nameof(Error):
                    xmlSerializer = new XmlSerializer(typeof(T));
                    xmlSerializer.Serialize(writer, content);
                    break;
                case nameof(Urls):
                    foreach (Url url in (IList<Url>)content)
                    {
                        xmlSerializer = new XmlSerializer(typeof(Url));
                        xmlSerializer.Serialize(writer, url);
                    }
                    break;
            }
            writer.WriteStartElement("ver");
            writer.WriteValue(ver);
            writer.WriteEndElement();
        }
    }

    [XmlRoot(ElementName = "")]
    public class Urls : ICollection, IEnumerable, IList<Url>, IXmlSerializable
    {
        public Urls()
        {
            _List = new List<Url>();
        }

        [XmlIgnore]
        public Url this[int index] { get => ((IList<Url>)_List)[index]; set => ((IList<Url>)_List)[index] = value; }

        public int Count => ((IList<Url>)_List).Count;

        public bool IsReadOnly => ((IList<Url>)_List).IsReadOnly;

        public object SyncRoot => ((ICollection)_List).SyncRoot;

        public bool IsSynchronized => ((ICollection)_List).IsSynchronized;

        private List<Url> _List
        {
            get;
            set;
        }

        #region Interface members
        public void Add(Url item)
        {
            ((IList<Url>)_List).Add(item);
        }

        public void Clear()
        {
            ((IList<Url>)_List).Clear();
        }

        public bool Contains(Url item)
        {
            return ((IList<Url>)_List).Contains(item);
        }

        public void CopyTo(Url[] array, int arrayIndex)
        {
            ((IList<Url>)_List).CopyTo(array, arrayIndex);
        }

        public void CopyTo(Array array, int index)
        {
            ((ICollection)_List).CopyTo(array, index);
        }

        public IEnumerator<Url> GetEnumerator()
        {
            return ((IList<Url>)_List).GetEnumerator();
        }

        public int IndexOf(Url item)
        {
            return ((IList<Url>)_List).IndexOf(item);
        }

        public void Insert(int index, Url item)
        {
            ((IList<Url>)_List).Insert(index, item);
        }

        public bool Remove(Url item)
        {
            return ((IList<Url>)_List).Remove(item);
        }

        public void RemoveAt(int index)
        {
            ((IList<Url>)_List).RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<Url>)_List).GetEnumerator();
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {

        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            foreach (Url url in _List)
            {
                var xmlSerializer = new XmlSerializer(typeof(Url));
                xmlSerializer.Serialize(writer, url);
            }
        }
        #endregion
    }

    [XmlType("total", Namespace = "", IncludeInSchema = false)]
    public class Total
    {
        [XmlText()]
        public int data;

        public Total()
        {
            data = default(int);
        }

        public Total(int _total)
        {
            data = _total;
        }
    }

    [XmlType("error", Namespace = "", IncludeInSchema = false)]
    public class Error
    {
        [XmlText()]
        public string data;

        public Error()
        {
            data = default(string);
        }

        public Error(string _error)
        {
            data = _error;
        }
    }

    [XmlType(IncludeInSchema = false, Namespace = "", TypeName = "url")]
    public class Url
    {
        [XmlText()]
        public string uri;
        [XmlAttribute()]
        public string replyId;
        [XmlAttribute()]
        public string fileId;
        [XmlAttribute()]
        public string timestamp;
        
        public Url()
        {
            this.replyId = "";
            this.uri = "";
            this.fileId = "";
            this.timestamp = "";
        }

        public Url(string replyId = "", string uri = "")
        {
            this.replyId = replyId;
            this.uri = uri;
        }

        public string Uri
        {
            get => this.uri;            
        }

        public string ReplyId
        {
            get => this.replyId;
        }

        public string FileId
        {
            get => this.fileId;
        }

        public Uri CreateUri()
        {
            return new Uri(this.uri);
        }
    }

    public class FormData : MemoryStream
    {
        private string boundary = Guid.NewGuid().ToString();

        public string Boundary { get => boundary; }

        public FormData():base()
        {
        }

        public FormData(Egais.Entities.WB_DOC_SINGLE_01.Documents doc) : base()
        {
            this.AppendFormDataElement(doc);
        }
        
        public void AppendFormDataElement(Egais.Entities.WB_DOC_SINGLE_01.Documents doc)
        {
            var streamWriter = new StreamWriter(this);
            {
                var xmlSerializer = new XmlSerializer(doc.GetType());
                streamWriter.WriteLine(String.Format("\r\n--{0}\r\n", Boundary));
                streamWriter.WriteLine("Content-Disposition: form-data; name=\"xml_file\"; filename=\"doc.xml\"\r\nContent-Type: xml\r\n\r\n");
                streamWriter.Flush();
                xmlSerializer.Serialize(this, doc);
                streamWriter.WriteLine(string.Format("\r\n--{0}--\r\n\r\n", Boundary));
                streamWriter.Flush();
                this.Seek(0, System.IO.SeekOrigin.Begin);
            }
        }
    }
}