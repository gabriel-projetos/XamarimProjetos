﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppGallery.Pages.Navigation_page
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Conteudo01 : ContentPage
    {
        public Conteudo01()
        {
            InitializeComponent();
        }
        
        private async void AbrirPagina02(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Conteudo02());
        }
    }
}