﻿using AppProdutos.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppProdutos
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new ProdutosView();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
