using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace HLP.WebServices
{
    public class Cep
    {

        public Endereco BuscaEndereco(string Cep)
        {
            Endereco en = null;
            try
            {
                System.Net.ServicePointManager.Expect100Continue = false;

                WebCEP service = new WebCEP(Cep);
                en = new Endereco();
                en.Logradouro = service.Lagradouro;
                en.Bairro = service.Bairro;
                en.Cidade = service.Cidade;
                en.Uf = service.UF;
                return en;

            }
            catch (Exception ex)
            {
                return en;
            }
        }
    }
}
