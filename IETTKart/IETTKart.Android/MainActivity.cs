using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Nfc;
using Android.OS;
using Poz1.NFCForms.Abstract;
using Poz1.NFCForms.Droid;
using System;
using Xamarin.Forms;

namespace IETTKart.Droid
{
    [Activity(Label = "E-Kart", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [IntentFilter(new[] { NfcAdapter.ActionTechDiscovered })]
    [MetaData(NfcAdapter.ActionTechDiscovered, Resource = "@xml/nfc")]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public NfcAdapter NFCdevice;
        public NfcForms x;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            NfcManager NfcManager = (NfcManager)Android.App.Application.Context.GetSystemService(Context.NfcService);
            NFCdevice = NfcManager.DefaultAdapter;

            DependencyService.Register<INfcForms, NfcForms>();
            x = Xamarin.Forms.DependencyService.Get<INfcForms>() as NfcForms;

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (NFCdevice != null)
            {
                var intent = new Intent(this, GetType()).AddFlags(ActivityFlags.SingleTop);
                NFCdevice.EnableForegroundDispatch
                (
                    this,
                    PendingIntent.GetActivity(this, 0, intent, 0),
                    new[] { new IntentFilter(NfcAdapter.ActionTechDiscovered) },
                    new String[][] {new string[] {
                            NFCTechs.Ndef,
                        },
                        new string[] {
                            NFCTechs.MifareClassic,
                        },
                    }
                );
            }
        }

        protected override void OnPause()
        {
            base.OnPause();
            NFCdevice.DisableForegroundDispatch(this);
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            x.OnNewIntent(this, intent);
        }
    }
}

