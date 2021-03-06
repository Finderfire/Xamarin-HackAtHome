using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using HackAtHome.Entities;

namespace HackAtHome.SAL
{
    public class MicrosoftServiceClient
    {
        //Se declara un cliente para acceder al servicio Mobile
        MobileServiceClient Client;
        
        //Objeto para realizar operaciones con las tablas del servicio Mobile
        private IMobileServiceTable<LabItem> LabItemTable;

        /// <summary>
        /// Env�a una evidencia.
        /// </summary>
        /// <param name="userEvidence">Objeto con los datos de la evidencia.</param>
        /// <returns></returns>
        public async Task SendEvidence(LabItem userEvidence)
        {
            Client =
                new MobileServiceClient(@"http://xamarin-diplomado.azurewebsites.net/");
            LabItemTable = Client.GetTable<LabItem>();
            await LabItemTable.InsertAsync(userEvidence);
        }
    }
}