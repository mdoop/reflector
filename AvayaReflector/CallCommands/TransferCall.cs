using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AvayaReflector.CallCommands
{
    [Serializable]
    public class TransferCall : CallCommand
    {
        [XmlElement(ElementName = "TransferExtension")]
        public string TransferExtension { get; set; }

        public override void Request(AvayaTsapiDLL.AvayaTsapiWrapper avayaController)
        {
            bool initiateResult;
            bool completeResult;

            if (avayaController != null)
            {
                //Initiate the Transfer
                initiateResult = avayaController.InitiateTransfer(this.Extension, this.TransferExtension);
                if (initiateResult)
                {
                    //Sleep for 3 seconds, otherwise the response is not returned.
                    System.Threading.Thread.Sleep(3000);
                    //Complete the Transfer
                    completeResult = avayaController.CompleteTransfer(this.Extension, this.TransferExtension);
                }
            }
        }
    }
}
