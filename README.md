# EGAIS-WCF-Client
Клиент УТМ ЕГАИС с использованием WCF 3.5

Egais.Entities сгенерирован с помощью скрипта xsd2code.ps1

Для тестирования и отладки использовались Fiddler и Node-RED (смотри EgaisUtmFlow.json).

Пример:

UtmClient utmClient = new UtmClient("IUtm2");
var getOutResponse = utmClient.GetOut();
if (getOutResponse != null)
{
    foreach (Url url in getOutResponse.Content)
    {
        var doc = utmClient.GetDocumentByUri(new Uri(url.Uri)); 
        //doc.Document.ItemElementName тип документа
    }
}
utmClient.Close();
