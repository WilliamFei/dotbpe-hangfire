// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: ProtobufObjectfactory

#region Designer generated code

using Google.Protobuf;
using DotBPE.Protobuf;

namespace Hangfire.Consumer
{
    public class ProtobufObjectFactory:IProtobufObjectFactory
    {
        public IMessage GetRequestTemplate(int serviceId, int messageId)
        {

            if (serviceId == 2000 && messageId == 1)
            {
                return new CommonReq();
            }

            return null;
        }

        public IMessage GetResponseTemplate(int serviceId, int messageId)
        {

            if (serviceId == 2000 && messageId == 1)
            {
                return new VoidRes();
            }

            return null;
        }
    }
}

#endregion Designer generated code