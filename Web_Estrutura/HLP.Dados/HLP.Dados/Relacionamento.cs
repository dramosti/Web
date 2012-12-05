using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HLP.Dados
{
    public class Relacionamento
    {

        private string sCampoPai;
        private string sCampoFilho;

        public string CampoPai
        {
            get
            {
                return sCampoPai;
            }
            set
            {
                sCampoPai = value;
            }
        }

        public string CampoFilho
        {
            get
            {
                return sCampoFilho;
            }
            set
            {
                sCampoFilho = value;
            }
        }

        public Relacionamento(string sCampoPai, string sCampoFilho)
        {
            this.sCampoPai = sCampoPai;
            this.sCampoFilho = sCampoFilho;
        }

    }
}
