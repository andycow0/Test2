using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Ancestor.Core;

namespace AncestorUnitTestProject.Models
{
    public class RLABINDX : IModel
	{
        [Required()]
        [StringLength(10)] 
	    [DisplayName("檢驗編號")]
		public string LABNO { get; set; } 
        [StringLength(5)] 
	    [DisplayName("檢體")]
		public string SPCM { get; set; } 
        [StringLength(30)] 
	    [DisplayName("檢體說明")]
		public string SPCMDESC { get; set; } 
        [StringLength(8)] 
	    [DisplayName("採檢日期")]
		public string CLTDAT { get; set; } 
        [StringLength(4)] 
	    [DisplayName("採檢時間")]
		public string CLTTM { get; set; } 
        [StringLength(3)] 
	    [DisplayName("採檢間隔時間")]
		public string CLTITVLTM { get; set; } 
        [StringLength(10)] 
	    [DisplayName("採檢地點")]
		public string CLTSITE { get; set; } 
        [StringLength(8)] 
	    [DisplayName("登記日期")]
		public string BKGDAT { get; set; } 
        [StringLength(4)] 
	    [DisplayName("登記時間")]
		public string BKGTM { get; set; } 
        [StringLength(8)] 
	    [DisplayName("收件日期")]
		public string RCVDAT { get; set; } 
        [StringLength(4)] 
	    [DisplayName("收件時間")]
		public string RCVTM { get; set; } 
        [StringLength(3)] 
	    [DisplayName("收件人員代號")]
		public string RCVRID { get; set; } 
        [StringLength(2)] 
	    [DisplayName("收件異常代號")]
		public string RCVABID { get; set; } 
        [StringLength(1)] 
	    [DisplayName("補建檔註記")]
		public string RAGFMK { get; set; } 
        [StringLength(1)] 
	    [DisplayName("急件註記")]
		public string STTMK { get; set; } 
        [StringLength(8)] 
	    [DisplayName("醫囑單編號")]
		public string ODRSHNO { get; set; } 
        [StringLength(5)] 
	    [DisplayName("醫囑科別")]
		public string ODRDPT { get; set; } 
        [StringLength(5)] 
	    [DisplayName("醫囑醫師")]
		public string ODRDR { get; set; } 
        [StringLength(8)] 
	    [DisplayName("醫囑日期")]
		public string ODRDAT { get; set; } 
        [StringLength(4)] 
	    [DisplayName("醫囑時間")]
		public string ODRTM { get; set; } 
        [StringLength(13)] 
	    [DisplayName("來源號碼")]
		public string SOURNO { get; set; } 
        [StringLength(3)] 
	    [DisplayName("健保卡號")]
		public string NHICRDNO { get; set; } 
        [StringLength(1)] 
	    [DisplayName("病患來源")]
		public string PTSOUR { get; set; } 
        [StringLength(1)] 
	    [DisplayName("病患保險別")]
		public string PTINSUID { get; set; } 
        [Required()]
        [StringLength(12)] 
	    [DisplayName("病歷號")]
		public string CHTNO { get; set; } 
        [StringLength(10)] 
	    [DisplayName("身份證字號")]
		public string IDNO { get; set; } 
        [StringLength(10)] 
	    [DisplayName("中文姓名")]
		public string CNM { get; set; } 
        [StringLength(24)] 
	    [DisplayName("英文姓名")]
		public string ENM { get; set; } 
        [StringLength(1)] 
	    [DisplayName("性別")]
		public string SEX { get; set; } 
        [StringLength(8)] 
	    [DisplayName("出生日期")]
		public string BRNDAT { get; set; } 
        [StringLength(2)] 
	    [DisplayName("血型")]
		public string BT { get; set; } 
        [StringLength(2)] 
	    [DisplayName("父親血型")]
		public string FABT { get; set; } 
        [StringLength(2)] 
	    [DisplayName("母親血型")]
		public string MOBT { get; set; } 
        [StringLength(2)] 
	    [DisplayName("懷孕週數")]
		public string APWEEKNUM { get; set; } 
        [StringLength(3)] 
	    [DisplayName("白血球篩檢")]
		public string LEUSCRN { get; set; } 
        [StringLength(1)] 
	    [DisplayName("用藥品時機")]
		public string USDGOCAS { get; set; } 
        [StringLength(5)] 
	    [DisplayName("一日尿液量")]
		public string X1DATURINQTY { get; set; } 
        [StringLength(1)] 
	    [DisplayName("長庚醫院感染註記")]
		public string CGHINFCMK { get; set; } 
        [StringLength(1)] 
	    [DisplayName("振幅(P/T)")]
		public string PEAK { get; set; } 
        [StringLength(5)] 
	    [DisplayName("體檢公司別")]
		public string HEALCOID { get; set; } 
        [StringLength(10)] 
	    [DisplayName("體檢序號")]
		public string HEALSQNO { get; set; } 
        [StringLength(200)] 
	    [DisplayName("檢驗結果說明")]
		public string LABRESUDESC { get; set; } 
        [StringLength(5)] 
	    [DisplayName("報告送達地點")]
		public string RPTDLVSITE { get; set; } 
        [StringLength(2)] 
	    [DisplayName("批價項目數")]
		public string CHRGITNUM { get; set; } 
        [StringLength(3)] 
	    [DisplayName("批價項目")]
		public string CHRGIT { get; set; } 
        [StringLength(3)] 
	    [DisplayName("前批價項目")]
		public string BFCHRGIT { get; set; } 
        [StringLength(20)] 
	    [DisplayName("體檢公司部門")]
		public string HEALCOIDDEPT { get; set; } 
        [StringLength(100)] 
	    [DisplayName("住址")]
		public string ADR { get; set; } 
        [StringLength(5)] 
	    [DisplayName("郵遞區號")]
		public string ZIP { get; set; } 
        [StringLength(1)] 
	    [DisplayName("檢驗組別")]
		public string LABGRP { get; set; } 
        [StringLength(10)] 
	    [DisplayName("檢體量")]
		public string SPCMQTY { get; set; } 
        [StringLength(1)] 
	    [DisplayName("鎖驗證註記")]
		public string LCKVRFMK { get; set; } 
        [StringLength(8)] 
	    [DisplayName("上鎖日期")]
		public string LCKDAT { get; set; } 
        [StringLength(4)] 
	    [DisplayName("上鎖時間")]
		public string LCKTM { get; set; } 
        [StringLength(5)] 
	    [DisplayName("上鎖人員代號")]
		public string LCKEMPID { get; set; } 
        [StringLength(10)] 
	    [DisplayName("上鎖人員姓名")]
		public string LCKEMPNM { get; set; } 
        [StringLength(8)] 
	    [DisplayName("簽收日期")]
		public string CHKRCVDAT { get; set; } 
        [StringLength(4)] 
	    [DisplayName("簽收時間")]
		public string CHKRCVTM { get; set; } 
        [StringLength(5)] 
	    [DisplayName("簽收人員代號")]
		public string CHKEMPID { get; set; } 
        [StringLength(10)] 
	    [DisplayName("簽收人員姓名")]
		public string CHKEMPNM { get; set; } 
        [StringLength(8)] 
	    [DisplayName("給藥日期")]
		public string DRGDAT { get; set; } 
        [StringLength(4)] 
	    [DisplayName("給藥時間")]
		public string DRGTM { get; set; } 
        [StringLength(5)] 
	    [DisplayName("FiO2")]
		public string FIO2 { get; set; } 
        [StringLength(100)] 
	    [DisplayName("臨床診斷說明")]
		public string CLIDIGDESC { get; set; } 
        [StringLength(10)] 
	    [DisplayName("檢驗結果異動人員")]
		public string LABRESUIPEMPNM { get; set; } 
        [StringLength(600)] 
	    [DisplayName("檢驗結果說明欄位")]
		public string LABRESUDESC2 { get; set; } 
        [StringLength(20)] 
	    [DisplayName("檢驗批號")]
		public string LOTNO { get; set; } 
        [StringLength(14)] 
	    [DisplayName("共用檢驗編號")]
		public string PRELABNO { get; set; }

        //為搜尋範圍用的參數
        [Browsable(false)]
        public string Rcvdat_Start { get; set; }
        [Browsable(false)]
        public string Rcvdat_End { get; set; }
        [Browsable(false)]
        public string Cltdat_Start { get; set; }
        [Browsable(false)]
        public string Cltdat_End { get; set; }

        //檢驗項目
        [DisplayName("檢驗項目 ")]
        public string LABIT { get; set; }
	
	}
}
