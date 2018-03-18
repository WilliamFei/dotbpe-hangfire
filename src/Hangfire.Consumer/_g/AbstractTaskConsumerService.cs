// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: taskconsumer_2000.proto

#region Designer generated code

using DotBPE.Protocol.Amp;
using DotBPE.Rpc;
using Google.Protobuf;
using System.Threading.Tasks;

namespace Hangfire.Consumer
{
    //start for class AbstractTaskConsumerService
    public abstract class TaskConsumerServiceBase : ServiceActor
    {
        protected override int ServiceId => 2000;


        public override Task<AmpMessage> ProcessAsync(AmpMessage req)
        {
            switch (req.MessageId)
            {

                //方法 TaskConsumerService.Excute
                case 1: return this.ProcessExcuteAsync(req);

                default: return base.ProcessNotFoundAsync(req);
            }
        }



        //调用委托
        private async Task<AmpMessage> ProcessExcuteAsync(AmpMessage req)
        {
            CommonReq request = null;

            if (req.Data == null)
            {
                request = new CommonReq();
            }
            else
            {
                request = CommonReq.Parser.ParseFrom(req.Data);
            }

            req.FriendlyServiceName = "TaskConsumerService.Excute";

            var result = await ExcuteAsync(request);
            var response = AmpMessage.CreateResponseMessage(req.ServiceId, req.MessageId);

            response.FriendlyServiceName = "TaskConsumerService.Excute";
            response.Code = result.Code;
            if (result.Data != null)
            {
                response.Data = result.Data.ToByteArray();
            }
            return response;
        }
        //抽象方法
        public abstract Task<RpcResult<VoidRes>> ExcuteAsync( CommonReq req);

    }

    //end for class AbstractTaskConsumerService
}

#endregion Designer generated code