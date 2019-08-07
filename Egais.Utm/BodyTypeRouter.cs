using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Egais.Utm
{
    public static class BodyTypeRouter
    {
        public static string Path(Egais.Entities.WB_DOC_SINGLE_01.ItemChoiceType docBodyType)
        {
            string path = "";

            switch(docBodyType)
            {
                case Entities.WB_DOC_SINGLE_01.ItemChoiceType.QueryClients:
                    path = "querypartner";
                    break;
                case Entities.WB_DOC_SINGLE_01.ItemChoiceType.ReplyClient:
                    path = "replypartner";
                    break;
                default:
                    {
                        path = docBodyType.ToString();
                        break;
                    }
            }

            return path;
        }
    }
}
