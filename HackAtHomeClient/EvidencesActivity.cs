using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using HackAtHome.CustomAdapters;
using HackAtHome.Entities;
using HackAtHome.SAL;

namespace HackAtHomeClient
{
    [Activity(Label = "@string/ApplicationName", Icon = "@drawable/Icon")]
    public class EvidencesActivity : Activity
    {           
        EvidencesAdapter EvidencesAdapter;
        EvidencesFragment DataEvidences;
        ListView lstEvidences;
        TextView txtFullName;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Evidences);

            lstEvidences = FindViewById<ListView>(Resource.Id.lstEvidences);
            txtFullName = FindViewById<TextView>(Resource.Id.txtFullName);

            //Se asigna el fragmento.
            DataEvidences = (EvidencesFragment)this.FragmentManager.FindFragmentByTag("DataEvidences");

            if(DataEvidences == null)
            {
                //al ser nulo se debe agregar el fragmento
                DataEvidences = new EvidencesFragment();
                var FragmentTransaction = this.FragmentManager.BeginTransaction();
                FragmentTransaction.Add(DataEvidences,"DataEvidences");
                FragmentTransaction.Commit();
            }

            //Se obtienen el listado de evidencias
            GetEvidences();

            //Se obtiene el nombre
            txtFullName.Text = DataEvidences.FullName;

            //Restaura el estado del listView si se guardó
            if(DataEvidences.ListPosition != null)
            {
                lstEvidences.OnRestoreInstanceState(DataEvidences.ListPosition);
                
            }


            lstEvidences.ItemClick += LstEvidences_ItemClick;
        }

        private void LstEvidences_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //se declara el Intent y se recuera los datos de la evidencia, enviandolos al nuevo activity.
            var ActivityIntent = new Android.Content.Intent(this, typeof(EvidenceDetailActivity));
            var Evidence = EvidencesAdapter[e.Position];

            ActivityIntent.PutExtra("FullName", DataEvidences.FullName);
            ActivityIntent.PutExtra("Token", DataEvidences.Token);
            ActivityIntent.PutExtra("EvidenceID", Evidence.EvidenceID);
            ActivityIntent.PutExtra("Title", Evidence.Title);
            ActivityIntent.PutExtra("Status", Evidence.Status);
            StartActivity(ActivityIntent);
        }

        private async void GetEvidences()
        {
            //Si no se comprueba la existencia del Token, se reemplazaria la informacion
            if (string.IsNullOrWhiteSpace(DataEvidences.Token))
            {
                //Se agrega a la data los datos obtenidos del 1er activity
                DataEvidences.FullName = Intent.GetStringExtra("FullName") ?? "El valor no está disponible";
                DataEvidences.Token = Intent.GetStringExtra("Token") ?? "El valor no está disponible";

                //Se obtiene el listado de evidencias desde ServiceClient            
                ServiceClient ServiceClient = new ServiceClient();
                try
                {
                    DataEvidences.Lista = await ServiceClient.GetEvidencesAsync(DataEvidences.Token);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            //Se asigna EvidencesAdapter a la lista según constructor
            EvidencesAdapter = new EvidencesAdapter
                 (
                    this,
                    DataEvidences.Lista,
                    Resource.Layout.EvidencesItem,
                    Resource.Id.txtLabTitle,
                    Resource.Id.txtLabStatus
                );
            lstEvidences.Adapter = EvidencesAdapter;
        }

        //se guarda el estado del listView
        protected override void OnPause()
        {
            lstEvidences = FindViewById<ListView>(Resource.Id.lstEvidences);
            DataEvidences.ListPosition = lstEvidences.OnSaveInstanceState();
            base.OnPause();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            lstEvidences = FindViewById<ListView>(Resource.Id.lstEvidences);
            DataEvidences.ListPosition = lstEvidences.OnSaveInstanceState();
            base.OnSaveInstanceState(outState);            
        }
    }
}