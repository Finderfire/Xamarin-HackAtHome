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
using Android.Webkit;
using HackAtHome.Entities;
using HackAtHome.SAL;

namespace HackAtHomeClient
{
    [Activity(Label = "@string/ApplicationName", Icon = "@drawable/Icon")]
    public class EvidenceDetailActivity : Activity
    {
        TextView txtEvidenceFullName;
        TextView txtEvidenceTitle;
        TextView txtEvidenceStatus;
        WebView webDescription;
        ImageView imgEvidenceImage;
        EvidenceDetailFragment DataEvidenceDetail;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EvidenceDetail);

            txtEvidenceFullName = FindViewById<TextView>(Resource.Id.txtEvidenceFullName);
            txtEvidenceTitle = FindViewById<TextView>(Resource.Id.txtEvidenceTitle);
            txtEvidenceStatus = FindViewById<TextView>(Resource.Id.txtEvidenceStatus);
            webDescription = FindViewById<WebView>(Resource.Id.webDescription);
            imgEvidenceImage = FindViewById<ImageView>(Resource.Id.imgEvidenceImage);

            //Se asigna el fragment
            DataEvidenceDetail = (EvidenceDetailFragment)this.FragmentManager.FindFragmentByTag("DataEvidenceDetail");
            if(DataEvidenceDetail == null)
            {
                DataEvidenceDetail = new EvidenceDetailFragment();
                var FragmentTransaction = this.FragmentManager.BeginTransaction();
                FragmentTransaction.Add(DataEvidenceDetail, "DataEvidenceDetail");
                FragmentTransaction.Commit();
            }

            //Se Obtienen los datos
            GetData();

            

        }

        //Obtenemos la data del detalle
        private async void GetData()
        {
            if (DataEvidenceDetail.Evidence == null)
            {
                //Recupera datos de la Evidencia
                DataEvidenceDetail.Evidence = new Evidence()
                {
                    EvidenceID = Intent.GetIntExtra("EvidenceID", 0),
                    Title = Intent.GetStringExtra("Title"),
                    Status = Intent.GetStringExtra("Status")
                };

                //Recupera datos  de autenticacion
                DataEvidenceDetail.FullName = Intent.GetStringExtra("FullName");
                DataEvidenceDetail.Token = Intent.GetStringExtra("Token");

                //Se obtiene el detalle de la evidencia desde ServiceClient
                ServiceClient ServiceClient = new ServiceClient();
                DataEvidenceDetail.EvidenceDetail = await ServiceClient.GetEvidenceByIDAsync
                (
                    DataEvidenceDetail.Token,
                    DataEvidenceDetail.Evidence.EvidenceID
                );

            }
            //Cargamos los datos guardados
            
            txtEvidenceFullName.Text = DataEvidenceDetail.FullName;
            txtEvidenceTitle.Text = DataEvidenceDetail.Evidence.Title;
            txtEvidenceStatus.Text = DataEvidenceDetail.Evidence.Status;
            webDescription.LoadDataWithBaseURL(null, $"<html><head><style type=\"text/css\">body{{color: #BDBDBD;font-size: 10pt }}</style></head><body>" +
                $"{ DataEvidenceDetail.EvidenceDetail.Description}</body></html>", "text/html", "utf-8", null);
            webDescription.SetBackgroundColor(Android.Graphics.Color.Transparent);
            Koush.UrlImageViewHelper.SetUrlDrawable(imgEvidenceImage, DataEvidenceDetail.EvidenceDetail.Url);
            
        }
    }
}