﻿using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using app1_testeDrive.Midia;
using app1_testeDrive.Droid;
using Xamarin.Forms;
using Android.Content;
using Android.Provider;
using Java.IO;
using Android.Util;
using System.IO;
using Android.Graphics;
using Android;


//Assembly: Marcado para ser usado como uma dependencia pelo dependencyService
[assembly: Xamarin.Forms.Dependency(typeof(MainActivity))]
namespace app1_testeDrive.Droid
{
    [Activity(Label = "app1_testeDrive", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]

    //Responsável por rodar o xamarim
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
         , ICamera
    {
        //escopo da classe
        static Java.IO.File arquivoImagem;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(builder.Build());

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        //É chamado pelo masterViewModel como uma dependencia
        public void TirarFoto()
        {
            //Intenção/ação de abrir camera para capturar imagem
            Intent intent = new Intent(MediaStore.ActionImageCapture);
          
            arquivoImagem = PegarArquivoImagem();

            //Armazenar dados extra 
            //Passando uma inteção com dados extra do tipo ExtraOutput
            //ExtraOutput: Saída de dados é aonde será guardado a captura final da foto
            // :Local aonde será guardado
            intent.PutExtra(MediaStore.ExtraOutput,
                Android.Net.Uri.FromFile(arquivoImagem));

            //Activity: Atividade
            //Intent:Intenção de executar uma tarefa de outro aplicativo e com quais dados se quer trabalhar
            //Para acionar o activity da camera é preciso acionar atravez de um intent

            //Atravez do contexto do xamarim transformado em activity sabemos qual é a atividade
            //apartir da atividade local é possivel ativar a atividade externa(camera)
            var activity = Forms.Context as Activity;

            //Iniciar uma atividade com o intuito de obeter um resultado
            //(Intent, 'Código de requisição')
            //Usamos o código na volta para saber qual requisição é
            activity.StartActivityForResult(intent, 0);
        }

        private static Java.IO.File PegarArquivoImagem()
        {
            Java.IO.File arquivoImagem;
            //Diretorio aonde vai ficar salvo as imagems
            Java.IO.File diretorio = new Java.IO.File(
                Android.OS.Environment.GetExternalStoragePublicDirectory(
                    Android.OS.Environment.DirectoryPictures), "Imagens");

            //Verificar se o diretorio existe
            if (!diretorio.Exists())
            {
                //é criado toda a estrutura de pais das pastas
                diretorio.Mkdirs();
            }

            //diretorio é usado aqui tb para ser a base
            //aonde a imagem Minhafoto.jpg será gravada
            arquivoImagem =
                new Java.IO.File(diretorio, "MinhaFoto.jpg");
            return arquivoImagem;
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            Java.IO.File diretorio = new Java.IO.File(
                Android.OS.Environment.GetExternalStoragePublicDirectory(
                    Android.OS.Environment.DirectoryPictures), "Imagens");

            var resolucaoAntiga = SizeImagem(arquivoImagem.Path);



            ResizeImage(arquivoImagem.Path, diretorio.Path, 1500, 1200);
            //var altura = bitmap.Height;
            //var largura = bitmap.Width;
            //var Alterada_a_resolução = new System.Drawing.Size((int)bitmap.GetBitmapInfo().Height, (int)bitmap.GetBitmapInfo().Width);
            //SE TIRAR A FOTO e confirmar 
            if (resultCode == Result.Ok)
            {
                byte[] bytes;

                //Retorna uma stream onde é possivel ler os dados do arquivo
                using (var stream = new Java.IO.FileInputStream(arquivoImagem))
                {
                    //Convertendo a imagem em um array de bytes para usar o array na troca de mensagens com o projeto c#
                    bytes = new byte[arquivoImagem.Length()];

                    //transferir pro array de bytes o conteudo do arquivo de imagem
                    //stream le os dados do arquivo
                    stream.Read(bytes);
                }

                //Arquivo do tipo array de byte[]
                //instancia de arquivoImagem
                //Sobrepoe a imagem original no diretorio
                //envia a msg para a classe masterviewmodel
                MessagingCenter.Send<byte[]>(bytes, "FotoTirada");
            }
        }

        public Bitmap SizeImagem(string sourceFile)
        {
            var options = new BitmapFactory.Options()
            {
                InJustDecodeBounds = false,
                InPurgeable = true,
            };

            using (var image = BitmapFactory.DecodeFile(sourceFile, options))
            {
                var sourceSize = new System.Drawing.Size((int)image.GetBitmapInfo().Height, (int)image.GetBitmapInfo().Width);
                var teste = 5;

                return image;
            }
        }


        public void ResizeImage(string sourceFile, string targetFile, float maxWidth, float maxHeight)
        {
            var permissions = new string[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage };
            RequestPermissions(permissions, 77);
            Bitmap bitmapScaled;
            // First decode with inJustDecodeBounds=true to check dimensions
            var options = new BitmapFactory.Options()
            {
                InJustDecodeBounds = false,
                InPurgeable = true,
            };

            var image = BitmapFactory.DecodeFile(sourceFile, options);
            //if (image != null)
            //{
                var sourceSize = new System.Drawing.Size((int)image.GetBitmapInfo().Height, (int)image.GetBitmapInfo().Width);

                var maxResizeFactor = Math.Min(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);

                string targetDir = System.IO.Path.GetDirectoryName(targetFile);
                if (!Directory.Exists(targetDir))
                    Directory.CreateDirectory(targetDir);


                Java.IO.File diretorio = new Java.IO.File(
                        Android.OS.Environment.GetExternalStoragePublicDirectory(
                            Android.OS.Environment.DirectoryPictures), "Imagens");

                
                    var width = (int)(maxResizeFactor * sourceSize.Width);
                    var height = (int)(maxResizeFactor * sourceSize.Height);

                    bitmapScaled = Bitmap.CreateScaledBitmap(image, 4096, 3072, true);
                    var stream = new Java.IO.FileInputStream(arquivoImagem);    
                    using (Stream outStream = System.IO.File.Create($"{targetDir}/Testes.jpg"))
                    {
                        if (targetFile.ToLower().EndsWith("png"))
                            bitmapScaled.Compress(Bitmap.CompressFormat.Png, 100, outStream);
                        else
                            bitmapScaled.Compress(Bitmap.CompressFormat.Jpeg, 80, outStream);

                         var bytesss = bitmapScaled.ByteCount;
                        var teste = 1;
                        //return bitmapScaled;
                    }
                    //bitmapScaled.Recycle();


        }
    }
}