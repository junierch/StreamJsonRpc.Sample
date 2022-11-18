using System;
using System.Threading.Tasks;
using System.Threading;
using StreamJsonRpc;

internal class Server
{
    public static JsonRpc JsonRpcInstance { get; set; }


    public async Task<int> Add(int a, int b, CancellationToken cancellationToken)
    {
        // Log to STDERR so as to not corrupt the STDOUT pipe that we may be using for JSON-RPC.
        //Console.Error.WriteLine($"Received request: {a} + {b}");

        await JsonRpcInstance.NotifyAsync("notification/addtest", "From JsonRpc server");

        return a + b;
    }


    //Will be common code
    public InitializeResult initialize(string processId, string rootPath, string rootUri, ClientCapabilities capabilities, string trace, string workspaceFolders)
    {
        return new InitializeResult {
            capabilities = new ServerCapabilities {
                textDocumentSync = TextDocumentSyncKind.Incremental,
                definitionProvider = true,
                referencesProvider = false,
                documentFormattingProvider = true,
                documentRangeFormattingProvider = true,
                documentHighlightProvider = false,
                hoverProvider = true,
                completionProvider = new CompletionOptions {
                    resolveProvider = true,
                    triggerCharacters = new string[] { ".", "-", ":", "\\", "[", "\"" }
                },
                signatureHelpProvider = new SignatureHelpOptions {
                    triggerCharacters = new string[] { " ", "," }
                }
            }
        };
    }

    public void Shutdown()
    {

    }

    public Version Version()
    {
        return new Version(1, 1024);
    }



    public class InitializeRequest
    {
        /// <summary>
        /// Gets or sets the root path of the editor's open workspace.
        /// If null it is assumed that a file was opened without having
        /// a workspace open.
        /// </summary>
        public string RootPath { get; set; }
        public int ProcessId { get; set; }
        public string RootUri { get; set; }
        public string Trace { get; set; }
        public string workspaceFolders { get; set; }

        /// <summary>
        /// Gets or sets the capabilities provided by the client (editor).
        /// </summary>
        public ClientCapabilities Capabilities { get; set; }
    }

    public class ClientCapabilities
    {
    }

    public class InitializeResult
    {
        /// <summary>
        /// Gets or sets the capabilities provided by the language server.
        /// </summary>
        public ServerCapabilities capabilities { get; set; }
    }
    public class ServerCapabilities
    {
        public TextDocumentSyncKind? textDocumentSync { get; set; }

        public bool? hoverProvider { get; set; }

        public CompletionOptions completionProvider { get; set; }

        public SignatureHelpOptions signatureHelpProvider { get; set; }

        public bool? definitionProvider { get; set; }

        public bool? referencesProvider { get; set; }

        public bool? documentHighlightProvider { get; set; }

        public bool? documentFormattingProvider { get; set; }

        public bool? documentRangeFormattingProvider { get; set; }

        public bool? documentSymbolProvider { get; set; }

        public bool? workspaceSymbolProvider { get; set; }
    }

    /// <summary>
    /// Defines the document synchronization strategies that a server may support.
    /// </summary>
    public enum TextDocumentSyncKind
    {
        /// <summary>
        /// Indicates that documents should not be synced at all.
        /// </summary>
        None = 0,

        /// <summary>
        /// Indicates that document changes are always sent with the full content.
        /// </summary>
        Full,

        /// <summary>
        /// Indicates that document changes are sent as incremental changes after
        /// the initial document content has been sent.
        /// </summary>
        Incremental
    }

    public class CompletionOptions
    {
        public bool? resolveProvider { get; set; }

        public string[] triggerCharacters { get; set; }
    }

    public class SignatureHelpOptions
    {
        public string[] triggerCharacters { get; set; }
    }
}
