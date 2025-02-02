﻿using System;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;
using Nerdbank.Streams;
using StreamJsonRpc;

class Program
{
    static async Task<int> Main(string[] args)
    {
       //All code here is test code, production code will not look like this

       await RespondToRpcRequestsAsync(FullDuplexStream.Splice(Console.OpenStandardInput(), Console.OpenStandardOutput()), 0);
       
       return 0;
    }


    private static async Task RespondToRpcRequestsAsync(Stream stream, int clientId)
    {

        await Console.Error.WriteLineAsync($"Connection request #{clientId} received. Spinning off an async Task to cater to requests.");
        var serverRpc = new JsonRpc(stream);
        Server.JsonRpcInstance = serverRpc;

        serverRpc.AddLocalRpcTarget(new Server());


        serverRpc.StartListening();
        await Console.Error.WriteLineAsync($"JSON-RPC listener attached to #{clientId}. Waiting for requests...");


        await serverRpc.Completion;
        await Console.Error.WriteLineAsync($"Connection #{clientId} terminated.");
    }
}
