using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAO
{
    public class daoOcorrencias
    {
        /// <summary>
        /// fk -> CD_OCOR => tabela = tabocorr
        /// </summary>
        public string DS_OCOR { get; set; }

        public DateTime DT_OCOR { get; set; }

        /// <summary>
        /// observação Ocorrencia
        /// </summary>
        public string CD_OBS { get; set; }

        /// <summary>
        /// TEXTO
        /// </summary>
        public string DS_TEXTO { get; set; }


    }
}
