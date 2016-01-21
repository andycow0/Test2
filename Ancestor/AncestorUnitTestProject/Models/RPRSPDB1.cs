﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AncestorUnitTestProject.Models
{
    public class RPRSPDB1
    {
        [DisplayName("病歷號 ")]
        public string CHTNO { get; set; }
        [DisplayName("身分證號碼 ")]
        public string IDNO { get; set; }
        [DisplayName("中文姓名 ")]
        public string CNM { get; set; }
        [DisplayName("性別 ")]
        public string SEX { get; set; }
        [DisplayName("英文姓名 ")]
        public string ENM { get; set; }
        [DisplayName("出生日期 ")]
        public string BRNDAT { get; set; }
        [DisplayName("電話號碼 ")]
        public string TEL { get; set; }
        [DisplayName("初診日期 ")]
        public Nullable<DateTime> FRVSTDAT { get; set; }
        [DisplayName("區域號碼 ")]
        public string ARENO { get; set; }
        [DisplayName("最後看診日 ")]
        public Nullable<DateTime> FNLOPDVSDAT { get; set; }
        [DisplayName("最後劃回日 ")]
        public Nullable<DateTime> FNLCKINDAT { get; set; }
        [DisplayName("住院註記 ")]
        public string ADMMK { get; set; }
        [DisplayName("傳真號碼 ")]
        public string FAXNO { get; set; }
    }
}
