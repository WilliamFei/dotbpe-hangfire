// Generated by the protocol buffer compiler. DO NOT EDIT!
// source: taskconsumer_2000.proto
#region Designer generated code

using System;
using System.Threading.Tasks;
using DotBPE.Rpc;
using DotBPE.Protocol.Amp;
using DotBPE.Rpc.Exceptions;
using Google.Protobuf;
using DotBPE.Rpc.Client;

namespace Hangfire.Consumer {

    //start for class TaskConsumerServiceClient
    public sealed class TaskConsumerServiceClient : AmpInvokeClient
    {
        public TaskConsumerServiceClient(ICallInvoker<AmpMessage> callInvoker) : base(callInvoker)
        {

        }

        //同步方法
        public RpcResult<VoidRes> Excute(CommonReq req)
        {
            AmpMessage message = AmpMessage.CreateRequestMessage(2000, 1);

            message.FriendlyServiceName = "TaskConsumerService.Excute";


            message.Data = req.ToByteArray();
            var response = base.CallInvoker.BlockingCall(message);
            if (response == null)
            {
                throw new RpcException("error,response is null !");
            }
            var result = new RpcResult<VoidRes>();
            if (response.Code != 0)
            {
                result.Code = response.Code;
            }
            else if (response.Data == null)
            {
                result.Code = ErrorCodes.CODE_INTERNAL_ERROR;
            }
            else
            {
                result.Data = VoidRes.Parser.ParseFrom(response.Data);
            }
            return result;
        }

        public async Task<RpcResult<VoidRes>> ExcuteAsync(CommonReq req, int timeOut = 3000)
        {
            AmpMessage message = AmpMessage.CreateRequestMessage(2000, 1);
            message.FriendlyServiceName = "TaskConsumerService.Excute";
            message.Data = req.ToByteArray();
            var response = await base.CallInvoker.AsyncCall(message, timeOut);
            if (response == null)
            {
                throw new RpcException("error,response is null !");
            }
           var result = new RpcResult<VoidRes>();
            if (response.Code != 0)
            {
                result.Code = response.Code;
            }
            else if (response.Data == null)
            {
                result.Code = ErrorCodes.CODE_INTERNAL_ERROR;
            }
            else
            {
                result.Data = VoidRes.Parser.ParseFrom(response.Data);
            }

            return result;
        }

    }
    //end for class TaskConsumerServiceClient
}
#endregion