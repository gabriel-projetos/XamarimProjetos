﻿using app1_testeDrive.Models;
using app1_testeDrive.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace app1_testeDrive.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AgendamentoView : ContentPage
    {
        public AgendamentoViewModel ViewModel { get; set; }

        //Construtor
        public AgendamentoView(Veiculo veiculo)
        {
            InitializeComponent();
            this.ViewModel = new AgendamentoViewModel(veiculo);
            //Passando o contexto do Binding para a pagina, veiculo que esta sendo recebido
            this.BindingContext = this.ViewModel;
        }

        //qnd a pagina aparecer na tela, await precisa ser usado com async
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //Tipo de msg, argumentos quem está assinando, nome da msg
            MessagingCenter.Subscribe<Agendamento>(this, "Agendamento",
                async (msg) =>
                {
                var confirma = await DisplayAlert("Salvar Agendamento",
                "Confirmar Agendamento ?",
                "Sim", "Não");

                    if (confirma)
                    {
                        this.ViewModel.SalvarAgendamento();
                    }
                });

            //Quando a mensagem chegar em msg, é executado o codigo.
            MessagingCenter.Subscribe<Agendamento>(this, "SucessoAgendamento", (msg) =>
            {
                DisplayAlert("Agendamento", "Agendamento salvo com sucesso!", "OK");
            });

            MessagingCenter.Subscribe<ArgumentException>(this, "FalhaAgendamento", (msg) =>
            {
                DisplayAlert("Agendameto", "Falha ao agendar!", "OK");
            });
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //A gente cancela a assinatura para que a msg n seja executada varias vezes
            //para nao causar conflito
            MessagingCenter.Unsubscribe<Agendamento>(this, "Agendamento");

            MessagingCenter.Unsubscribe<Agendamento>(this, "SucessoAgendamento");
            MessagingCenter.Unsubscribe<ArgumentException>(this, "FalhaAgendamento");
        }
    }
}