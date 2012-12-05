using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HLP.WebServices
{
    public class Endereco
    {
        private string _Logradouro;

        public string Logradouro
        {
            get { return _Logradouro; }
            set { _Logradouro = value; }
        }
        private string _Bairro;

        public string Bairro
        {
            get { return _Bairro; }
            set { _Bairro = value; }
        }
        private string _Cidade;

        public string Cidade
        {
            get { return _Cidade; }
            set { _Cidade = value; }
        }
        private string _Uf;

        public string Uf
        {
            get { return _Uf; }
            set { _Uf = value; }
        }
    }
}
