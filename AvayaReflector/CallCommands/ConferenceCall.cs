using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AvayaReflector.CallCommands
{
    public class ConferenceCall : CallCommand
    {
        [XmlElement(ElementName = "ConferenceExtension")]
        public string ConferenceExtension { get; set; }

        public override void Request(AvayaTsapiDLL.AvayaTsapiWrapper avayaController)
        {
            bool initiateConferenceResult;
            bool completeConferenceResult;
            try
            {
                if (avayaController != null)
                {
                    initiateConferenceResult = avayaController.InitiateConference(this.Extension, this.ConferenceExtension);
                    if (initiateConferenceResult)
                    {
                        System.Threading.Thread.Sleep(2000);
                        completeConferenceResult = avayaController.CompleteConference(this.Extension, this.ConferenceExtension);
                    }
                }
            }
            catch (Exception)
            {
                //throw the exception back up to the application.
                throw new Exception("Conference Call Failed To Execute.");
            }
        }
    }
}
