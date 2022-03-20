using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
namespace BunnyMonitor.Shared
{
    public partial class Loader : ComponentBase
    {
        protected override async Task OnInitializedAsync()
        {
            await Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(1);
                    StateHasChanged();
                }
            });
        }
    }
}