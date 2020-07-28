using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace WasmJSInterop
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            var host = builder.Build();

            JSInvoker.Current.JSRuntime = host.Services.GetRequiredService<IJSRuntime>();
            JSInvoker.Current.JSInProcessRuntime = (JSInProcessRuntime)JSInvoker.Current.JSRuntime;

            await host.RunAsync();
        }
    }

    public class JSInvoker
    {
        public static JSInvoker Current { get; } = new JSInvoker();

        public IJSRuntime JSRuntime { get; set; }

        public IJSInProcessRuntime JSInProcessRuntime { get; set; }
    }

    public class DotNetMethodProvider
    {
        [JSInvokable] // DotNet.invokeMethodAsync('WasmJSInterop', 'Test', 3)
        public static async Task Test(int value)
        {
            value *= 2;

            Console.WriteLine(value);

            JSInvoker.Current.JSInProcessRuntime.InvokeVoid("localStorage.setItem", "value", value);
        }
    }
}

