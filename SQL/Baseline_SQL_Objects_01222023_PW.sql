

/****** Object:  Table [dbo].[_aac_CR_load_b]    Script Date: 1/22/2023 8:55:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE TABLE [dbo].[_AacCRImpErrorLog](
	[ErrorId] [int] IDENTITY(1,1) NOT NULL,
	[Severity] [int] NULL,
	[BatchID] [int] NULL,
	[Phase] [varchar](255) NULL,
	[ErrorCode] [varchar](255) NULL,
	[TrackingNumber] [varchar](30) NULL,
	[IdentifierCode] [varchar](255) NULL,
	[ErrorValue] [varchar](255) NULL,
	[ErrorDesc] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO



CREATE TABLE [dbo].[_aac_CR_load_b](
	[ApplicationType] [varchar](1) NULL,
	[ReceiptDate] [varchar](30) NULL,
	[BankCode] [varchar](10) NULL,
	[ReceiptType] [varchar](10) NULL,
	[CheckNum] [varchar](20) NULL,
	[DrawnBy] [varchar](100) NULL,
	[Payor] [varchar](30) NULL,
	[Client] [varchar](30) NULL,
	[MatterID] [varchar](30) NULL,
	[InvoiceNo] [varchar](30) NULL,
	[Amt] [varchar](30) NULL,
	[TrustID] [varchar](30) NULL,
	[GLAcct] [varchar](30) NULL,
	[GLOffc] [varchar](30) NULL,
	[GLDept] [varchar](30) NULL,
	[CreditType] [varchar](1) NULL,
	[DistDescription] [varchar](500) NULL,
	[BatchID] [int] NULL,
	[LineSeq] [int] NULL,
	[TranSeq] [int] NULL,
	[TranLineSeq] [int] NULL,
	[_SourceFile] [varchar](256) NULL,
	[_ImportStatus] [char](1) NULL,
	[_StagingUser] [varchar](60) NULL,
	[_StagingDate] [datetime] NULL,
	[_ImportDate] [datetime] NULL,
	[RACN] [uniqueidentifier] NULL,
	[TranRef] [varchar](40) NULL,
	[_SESSION] [int] NULL,
	[_AllocationMethod] [char](1) NULL
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[_AAC_CRMAP]    Script Date: 1/22/2023 8:55:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[_AAC_CRMAP](
	[RowUno] [int] IDENTITY(1,1) NOT NULL,
	[MapCode] [varchar](10) NULL,
	[ApplicationType] [varchar](60) NULL,
	[StageTable] [varchar](60) NULL,
	[SourceColumnLabel] [varchar](60) NULL,
	[TargetColumn] [varchar](60) NULL,
	[DataType] [char](1) NULL
) ON [PRIMARY]
GO

/****** Object:  Index [_AAC_CRMAP_ClusteredIndex]    Script Date: 1/22/2023 8:55:24 PM ******/
CREATE UNIQUE CLUSTERED INDEX [_AAC_CRMAP_ClusteredIndex] ON [dbo].[_AAC_CRMAP]
(
	[RowUno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[_aac_CRMAPZ]    Script Date: 1/22/2023 8:55:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[_aac_CRMAPZ](
	[MapCode] [varchar](8) NULL,
	[MapDesc] [varchar](40) NULL,
	[FileMask] [varchar](40) NULL,
	[FileType] [varchar](10) NULL,
	[Inactive] [char](1) NULL,
	[ReceiptID] [varchar](60) NULL,
	[TargetTable] [varchar](60) NULL,
	[XLSheetName] [varchar](max) NULL,
	[FirstDataRow] [int] NULL,
	[SchemaName] [varchar](60) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


create table _aac_CR_Audit_b (
 Row_Uno integer identity(1,1)
,BatchID integer
,ProcessDate datetime
,Severity integer
,CRSession integer
,CRReceipt_Num integer
,TranType char(3)
,Amount numeric (17,7)
,BillNum integer
,MatterUno	integer
,[Comment] varchar(max)

)


/****** Object:  Sequence [Sequence].[Import_Num]    Script Date: 1/22/2023 8:50:45 PM ******/
CREATE SEQUENCE [Sequence].[Import_Num] 
 AS [bigint]
 START WITH 1   --<-- Set first iomport number here
 INCREMENT BY 1
 MINVALUE -9223372036854775808
 MAXVALUE 9223372036854775807
 CACHE  500 
GO





/****** Object:  UserDefinedFunction [dbo].[FN_RemoveNonNumeric]    Script Date: 1/22/2023 8:50:09 PM ******/
DROP FUNCTION [dbo].[FN_RemoveNonNumeric]
GO

/****** Object:  UserDefinedFunction [dbo].[FN_RemoveNonNumeric]    Script Date: 1/22/2023 8:50:09 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- Created this on Dechert Prod on 4/7/2021

CREATE FUNCTION [dbo].[FN_RemoveNonNumeric] (@Input NVARCHAR(512))
RETURNS NVARCHAR(512)
AS
BEGIN
DECLARE @Trimmed NVARCHAR(512)

SELECT @Trimmed = @Input

WHILE PATINDEX('%[^0-9.]%', @Trimmed) > 0
    SELECT @Trimmed = REPLACE(@Trimmed, SUBSTRING(@Trimmed, PATINDEX('%[^0-9.]%', @Trimmed), 1), '')

RETURN @Trimmed
END

GO



/****** Object:  StoredProcedure [dbo].[SpAacCRLogProcedure]    Script Date: 1/22/2023 8:59:48 PM ******/
DROP PROCEDURE [dbo].[SpAacCRLogProcedure]
GO



/****** Object:  StoredProcedure [dbo].[SpAacCRRaisError]    Script Date: 1/22/2023 9:00:48 PM ******/
DROP PROCEDURE [dbo].[SpAacCRRaisError]
GO

/****** Object:  StoredProcedure [dbo].[SpAacCRRaisError]    Script Date: 1/22/2023 9:00:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


create procedure [dbo].[SpAacCRRaisError]
as
	DECLARE @errorMessage NVARCHAR(4000), @errorSeverity INT, @errorState INT;

    SELECT
        @errorMessage = N'Procedure: ' + ERROR_PROCEDURE() + ';' + ERROR_MESSAGE(),
        @errorSeverity = ERROR_SEVERITY(),
        @errorState = ERROR_STATE();

    RAISERROR (@errorMessage, -- Message text.
               @errorSeverity, -- Severity.
               @errorState -- State.
               );

GO




create PROCEDURE [dbo].[SpAacCRLogProcedure]
 @ObjectID       INT,
 @DatabaseID     INT = NULL,
 @AdditionalInfo NVARCHAR(1000) = NULL,
 @ProcedurePhase nvarchar(10) = NULL,
 @BatchID	INT = NULL,
 @errMessage NVARCHAR(4000) = NULL,
 @errSeverity INT = NULL,
 @errState INT = NULL


AS
BEGIN
 SET NOCOUNT ON;

	DECLARE @ProcedureName NVARCHAR(400);
	DECLARE @errorLine INT;

 SELECT
  @DatabaseID = COALESCE(@DatabaseID, DB_ID())

 SET @ProcedureName = OBJECT_NAME(@ObjectID);


 INSERT INTO _AacCRProcedureLog
 (
  DatabaseID,
  ObjectID,
  ProcedureName,
  ProcedurePhase,
  ErrorLine,
  ErrorMessage,
  ErrorSeverity,
  AdditionalInfo,
  BatchID
 )
 SELECT
  @DatabaseID,
  @ObjectID,
  @ProcedureName,
  @ProcedurePhase,
  @errorLine,
  @errMessage,
  @errSeverity,
  @AdditionalInfo,
  @BatchID;
END

GO





/****** Object:  StoredProcedure [dbo].[SpAacCRPreValidateData_B]    Script Date: 1/22/2023 8:49:14 PM ******/
DROP PROCEDURE [dbo].[SpAacCRPreValidateData_B]
GO

/****** Object:  StoredProcedure [dbo].[_spaac_ImportCR]    Script Date: 1/22/2023 8:49:14 PM ******/
DROP PROCEDURE [dbo].[_spaac_ImportCR]
GO

/****** Object:  StoredProcedure [dbo].[_spaac_CR_Import_Map]    Script Date: 1/22/2023 8:49:14 PM ******/
DROP PROCEDURE [dbo].[_spaac_CR_Import_Map]
GO

/****** Object:  StoredProcedure [dbo].[_spaac_Cr_Create_Session]    Script Date: 1/22/2023 8:49:14 PM ******/
DROP PROCEDURE [dbo].[_spaac_Cr_Create_Session]
GO

/****** Object:  StoredProcedure [dbo].[_spaac_CL_Credit_Application]    Script Date: 1/22/2023 8:49:14 PM ******/
DROP PROCEDURE [dbo].[_spaac_CL_Credit_Application]
GO

/****** Object:  StoredProcedure [dbo].[_spaac_MT_Credit_Application]    Script Date: 1/22/2023 8:49:14 PM ******/
DROP PROCEDURE [dbo].[_spaac_MT_Credit_Application]
GO

/****** Object:  StoredProcedure [dbo].[_spaac_Cr_Create_Receip]    Script Date: 1/22/2023 8:49:14 PM ******/
DROP PROCEDURE [dbo].[_spaac_Cr_Create_Receip]
GO

/****** Object:  StoredProcedure [dbo].[_spaac_Cr_Bill_Application]    Script Date: 1/22/2023 8:49:14 PM ******/
DROP PROCEDURE [dbo].[_spaac_Cr_Bill_Application]
GO

/****** Object:  StoredProcedure [dbo].[_spaac_Cr_Bill_Application]    Script Date: 1/22/2023 8:49:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE   proc [dbo].[_spaac_Cr_Bill_Application]
		 @ReceiptNum	integer
		,@BatchID		integer
		,@TranSeq		integer
		,@TranLineSeq	integer
		,@AmtApplied	money  OUTPUT
		,@MatterUno     integer = NULL
		,@AmountToAllocate numeric(17,4) = NULL
		,@Debug bit = 1
		--,@TestAllocationMethod char(1)
AS
--12/7/2022 SDN/AAC utilize new column - _allocationMethod
--  {blank} = treat as '1'
--	'1' = Apply ammount, shortpay applied top-down leaves AR, over pay will put balance into UnApplied bucket
--	'2' = Apply ammount, shortpay prorated across all matters, over pay will put balance into UnApplied bucket
--	'3' = Apply amount, if shortpay, settle AR in full and record difference to GL account for bank fees
--	'4' = Apply amount, if shortpay, settle AR in full and writeoff balance of bill

--  To move multimatter logic into this proc, need to identify all matters with AR and loop through each.  Use outstanding AR as basis for proration if indicated
--  Logic should hold if single matter bill.
-- CONSIDER multi-payor bills ???

select 'Begin [_spaac_Cr_Bill_Application] Batch:' + convert(varchar(10),@batchid) + ', TranSeq:' + convert(varchar(10),@TranSeq) + ', TranLineSeq:' + Convert(varchar(10),@TranLineSeq)

Set @AmtApplied = 0.00 -- Procedure will return applied amount in OUTPUT parameter

Declare @BillNum integer
Declare @CRAmt money
declare @BTU integer
Declare @PayorUno integer
Declare @AllocationMethod char(1) 
Declare @TranRef varchar(40)


declare @DefEmpl integer 
declare @DefDept varchar(10) 
declare @DefProf varchar(10)
declare @GLOffc varchar(10)
select  @DefEmpl = OVH_EMPL_UNO
       ,@DefDept = OVH_DEPT 
       ,@DefProf = OVH_PROF 
       ,@GLOffc  = DEF_OFFC      from glm_parms


declare @BankAcctUno integer = (select acct_uno from glm_chart where acct_code like '61460' and book = 1)

Declare @AR	numeric(17,10),@PmtPct numeric(17,10)
		,@ARFEES numeric(17,10),@ARHARD numeric(17,10),@ARSOFT numeric(17,10),@AROAF numeric(17,10),@AROAD numeric(17,10)
        ,@ARINT numeric(17,10),@ARTAX numeric(17,10),@ARPD numeric(17,10),@ARRET numeric(17,10)

Declare  @WO numeric(17,10), @WOFEES numeric(17,10),@WOHARD numeric(17,10),@WOSOFT numeric(17,10),@WOOAF numeric(17,10),@WOOAD numeric(17,10)
        ,@WOINT numeric(17,10),@WOTAX numeric(17,10),@WOPD numeric(17,10),@WORET numeric(17,10)

Declare @BLFEES	numeric(17,10),@BLHARD numeric(17,10),@BLSOFT numeric(17,10),@BLOAF numeric(17,10),@BLOAD numeric(17,10)
		,@BLINT numeric(17,10),@BLTAX numeric(17,10),@BLPD numeric(17,10),@BLRET numeric(17,10)

Declare  @UnallocatedCRAmt numeric(17,10), @UnallocatedAmt numeric(17,10),@AllocatedAmt numeric(17,10)
		,@allocatedFEES numeric(17,10),@allocatedHARD numeric(17,10),@allocatedSOFT numeric(17,10),@allocatedOAF numeric(17,10),@allocatedOAD numeric(17,10)
		,@allocatedINT numeric(17,10),@allocatedTAX numeric(17,10),@allocatedPD numeric(17,10),@allocatedRET numeric(17,10)

-- -------------------------------------------------------------------------------
/*
Process should 
Breakout amount into buckets based on allocation plan
1.	insert CR transaction row into BLT_BILL_AMT with allocted bucket amounts
2.	update BLT_BILLM to adjust AR amounts by subtracting paid amounts !! By Matter !!
3.	update BLT_BILLP to adjust AR amounts by subtracting paid amounts !! By Payor !!

	For Hard/Soft - separately
4.	insert into BLH_PAID_DISB CR Trans (Credit Amount) by Cost_Code, Matter, BLT_BILLED_DISB UNO
4.	insert into BLH_PAID_DISB WO Trans (Credit Amount) by Cost_Code, Matter, BLT_BILLED_DISB UNO  IF ALLOCATION METHOD IS 4

5.	update into BLH_BILLED_DISB CR Trans (PAID_CREDIT_TOT - allocated amount) by Cost_Code, Matter, BLT_BILLED_DISB UNO
5.	update into BLH_BILLED_DISB WO Trans (PAID_CREDIT_TOT - allocated amount) by Cost_Code, Matter, BLT_BILLED_DISB UNO   IF ALLOCATION METHOD IS 4
    
	For Fees
6.	insert into BLH_PAID_FEES CR Trans (Credit Amount) by Cost_Code, Matter, BLT_BILLED_FEES UNO
7.	update into BLH_BILLED_FEES CR Trans (PAID_CREDIT_TOT - allocated amount) by Cost_Code, Matter, BLT_BILLED_FEES UNO+

8a.	Create tran for bank charges if allocationmethod = 3 
8b.	Create Write Off transactions for bill underpayments and allocationmethod = 4
		
		
		Insert ACT_TRAN row for WO Using druno of bill, period of WO, reason_code
		Insert BLT_BILL_AMT for WO with amounts to bring AR to zero.  Druno of bill
		Update BLT_BILLM, AR should be zero (per matter) AR_Status = 'S'
		Update BLT_BILLP, AR should be zero (per matter) AR_Status = 'S'
		Update TBH_MATTER_SUMM increment WO_xxxx, AR_BAL_xxxx with writeoff amounts, Last_AR_PER = Period of WO (per matter)
		Update TBH_CLIENT_SUMM increment WO_xxxx, AR_BAL_xxxx with writeoff amounts, Last_AR_PER = Period of WO (per matter)
		Update BLH_BILLED_DISB (H & S), BLH_BILLED_FEES to increment PAID_CREDIT_TOT
		Insert BLH_PAID_DISB (H & S), BLH_PAID_FEES a WO tran with bill_credit_amt (o) , credit_amount = fees_credit to bring outstanding to zero
		Execute sp_cmstaxdetail {act_tran_uno}
		Execute SP_CMS_SC_CLEANUP {act_tran_uno}

		

9.	Create tran for Unapplied Cash under the general matter for the payor if CR Amount > AR Amount on bill

*/

-- ------------------ Get Distribution details from staging table ----------------
if @Debug = 1
BEGIN
select
	 a.invoiceno
	,convert(numeric(17,10),a.amt) 'Amt'
	,bl.TRAN_UNO
	,isnull(a._AllocationMethod,'1') '_AllocationMethod'
	,a.TranRef 
	from _aac_cr_load_b a
	left join blt_bill bl on a.invoiceno = bl.bill_num
	where batchid = @Batchid and transeq = @transeq and tranlineseq = @TranLineSeq 


--select  -- Can use this logic to reset billm/p balances on the fly
--	 @BTU = bl.TRAN_UNO
--	from _aac_cr_load_b a
--	left join blt_bill bl on a.invoiceno = bl.bill_num
--	where batchid = @Batchid and transeq = @transeq and tranlineseq = @TranLineSeq 
--exec [dbo].[##_Fix_BILLM_BILLP_AR]  @billuno = @BTU, @commit = 'Y', @details = 'N'
END

	select
	 @BillNum 	 = a.invoiceno
	,@CRAmt		 = convert(numeric(17,10),a.amt)
	,@BTU = bl.TRAN_UNO
	,@AllocationMethod = isnull(a._AllocationMethod,'1')
	,@TranRef = a.TranRef 

	--  select top 10 * 
	from _aac_cr_load_b a
	left join blt_bill bl on a.invoiceno = bl.bill_num
	where batchid = @Batchid and transeq = @transeq and tranlineseq = @TranLineSeq 
	-- 1/11/2023 Added test for bill not cancelled.  There could be >1 BLT_BILL row matching the bill number
	and exists (select 1 from blt_billm bm where bm.bill_tran_uno = bl.TRAN_UNO and ar_status <> 'X')


Set @UnallocatedCRAmt = @CRAMT -- initialize unallocated amouint to CR Amount
declare @AmountToApply numeric(17,4), @TranCRAmt numeric(17,4)
declare @MatterAR numeric(17,10), @TotalAR numeric(17,10), @MatterBilled numeric(17,10), @TotalBilled numeric(17,10)


--Get all matters for this bill
declare BM insensitive cursor for 
		select bm.matter_uno, bm.bill_tran_uno, bm.TOTAL_AR 'MatterAR', sum(bmt.total_ar) 'TotalAR', bm.TOTAL_BIL 'MatterBilled', sum(bmt.TOTAL_BIL) 'TotalBilled'  
			,m.client_uno 'PayorUno'
		from blt_billm bm 
		join blt_billm bmt on bm.bill_tran_uno = bmt.bill_tran_uno
		join hbm_matter m on bm.matter_uno = m.matter_uno
		--join blt_bill bl on bm.bill_tran_uno = bl.tran_uno 
		where bm.bill_tran_uno = @BTU --bill_num = @InvoiceNo and bl.LAST_BILL_ACTION <> 'REVR' 
		and bm.ar_status = 'O'
		group by bm.matter_uno, bm.bill_tran_uno, bm.TOTAL_AR , bm.TOTAL_BIL, m.client_uno 

open BM

declare @ThisIsMyTransaction bit = 0
	if @@trancount = 0
	BEGIN
		set @ThisIsMyTransaction = 1
		begin transaction 
	END



while 1=1
BEGIN -- Process each matter with AR for this bill
	fetch next from BM into @MatterUno, @BTU, @MatterAR, @TotalAR, @MatterBilled, @TotalBilled, @Payoruno
	if @@fetch_status = -1 break -- EXIT LOOP
	if @AllocationMethod = '1' 
			set @AmountToApply = case when @MatterAR >= @UnallocatedCRAmt then @UnallocatedCRAmt else @MatterAR end  -- Allocate lesser of AR or Unallocated amount available
	if @AllocationMethod in ( '2','4')
		Begin
			set @AmountToApply = @CRAmt * (@MatterAR /  @TotalAR)  --Percent based on Outstanding AR amounts
			if @AmountToApply > @MatterAR set @AmountToApply = @MatterAR  --  Don't pay more than due
			--set @AmountToApply = @CRAmt * (@MatterBilled / @TotalBilled  ) --Percent of CR amount based on Original billed amounts
		End
    if @AllocationMethod in ( '3')
			set @AmountToApply = @MatterAR  -- Clear total AR, remainder will be for Bank Fees or bill will be written off


	set @UnallocatedCRAmt = @UnallocatedCRAmt - @AmountToApply
    if @Debug = 1
		select @AllocationMethod 'AllocationMethod', @MatterAR '@MatterAR', @TotalAR '@TotalAr',  @MatterUno '@matterUno', @CRAmt '@CRAmt', @AmountToApply '@AmountToApply', @UnallocatedCRAmt '@UnallocatedCRAmt'
		, (@MatterBilled / @TotalBilled  ) 'BilledPct', (@MatterAR /  @TotalAR) 'ARPct'
    
--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==
--++--++--==--==--++--++--==--==--++                     Do The ledger Allocation                 ++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==
--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==
select
	 @ARFEES = sum(sign * FEES_AMT)
	,@ARHARD = sum(sign * HARD_AMT)
	,@ARSOFT = sum(sign * SOFT_AMT)
	,@AROAF	 = sum(sign * OAFEE_AMT)
	,@AROAD	 = sum(sign * OADISB_AMT)
	,@ARRET	 = sum(sign * RETAINER_AMT)
	,@ARPD	 = sum(sign * PREMDISC_AMT)
	,@ARTAX	 = sum(sign * TAX_AMT)
	,@ARINT	 = sum(sign * INTEREST_AMT)
	,@BLFEES = sum(case when tran_type = 'BL' then sign * FEES_AMT else 0.00 end)
	,@BLHARD = sum(case when tran_type = 'BL' then sign * HARD_AMT else 0.00 end)
	,@BLSOFT = sum(case when tran_type = 'BL' then sign * SOFT_AMT else 0.00 end)
from blt_bill_amt where  bill_tran_uno = @btu                                                                  and matter_uno = @MatterUno  --!!!!!!!!!!!!!!!!!!!!!!!!!--
and druno >= -1

if @Debug = 1 select  @ARFEES	'@ARFEES',@ARHARD	'@ARHARD'	,@ARSOFT	'@ARSOFT'	,@AROAF		'@AROAF	'	,@AROAD		'@AROAD	'	
                     ,@ARRET		'@ARRET	'	,@ARPD		'@ARPD	'	,@ARTAX		'@ARTAX	'	,@ARINT		'@ARINT	'	,@BLFEES	'@BLFEES'	
					 ,@BLHARD	'@BLHARD'	,@BLSOFT	'@BLSOFT'	

If @AmountToApply <> 0 --  iF @cramt IS ZERO -- NOTHING TO APPLY
BEGIN 
		------declare @ThisIsMyTransaction bit = 0
		------if @@trancount = 0
		------BEGIN
		------	set @ThisIsMyTransaction = 1
		------	begin transaction 
		------END

		--Allocate amounts to buckets based on allocation plan
		-- Allocation Priorities:	PRIO1	PRIO2	PRIO3	PRIO4	PRIO5	PRIO6	PRIO7
		--							HARD	SOFT	TAX		FEES	RTNR	OAD		OAF
		-- iF AR for bucket is zero, set allocation amount = zero
		-- If AR is less than or equal to unallocated amount, Set allocation - AR Amount; then continue allocation 
		-- If AR is Greater than allocation amount, apply available balance to bucket; then nothing left to allocate
		set @UnallocatedAmt = @AmountToApply
		if @ARHARD = 0.00 set @AllocatedHard = 0.00 else
		if @ARHARD <= @UnallocatedAmt begin SET @allocatedHARD = @ARHARD          SET @UnallocatedAmt = @UnallocatedAmt - @ARHARD END
									else begin SET @allocatedHARD = @UnallocatedAmt  SET @UnallocatedAmt = 0.00					  END   
		if @ARSOFT = 0.00 set @AllocatedSOFT = 0.00 else
		if @ARSOFT <= @UnallocatedAmt begin SET @allocatedSOFT = @ARSOFT          SET @UnallocatedAmt = @UnallocatedAmt - @ARSOFT END
									else begin SET @allocatedSOFT = @UnallocatedAmt  SET @UnallocatedAmt = 0.00					  END 
		if @ARTAX = 0.00 set @AllocatedTAX = 0.00 else
		if @ARTAX  <= @UnallocatedAmt begin SET @allocatedTAX = @ARTAX            SET @UnallocatedAmt = @UnallocatedAmt - @ARTAX  END
									else begin SET @allocatedTAX = @UnallocatedAmt   SET @UnallocatedAmt = 0.00					  END 
		if @ARFEES = 0.00 set @AllocatedFees = 0.00 else
		if @ARFEES <= @UnallocatedAmt begin SET @allocatedFEES = @ARFEES          SET @UnallocatedAmt = @UnallocatedAmt - @ARFEES END
									else begin SET @allocatedFEES = @UnallocatedAmt  SET @UnallocatedAmt = 0.00					  END 
		if @ARRET = 0.00 set @AllocatedRET = 0.00 else
		if @ARRET <= @UnallocatedAmt begin SET @allocatedRET = @ARRET             SET @UnallocatedAmt = @UnallocatedAmt - @ARRET  END
									else begin SET @allocatedRET = @UnallocatedAmt   SET @UnallocatedAmt = 0.00					  END 
		if @AROAD = 0.00 set @AllocatedOAD = 0.00 else
		if @AROAD <= @UnallocatedAmt begin SET @allocatedOAD = @AROAD             SET @UnallocatedAmt = @UnallocatedAmt - @AROAD  END
									else begin SET @allocatedOAD = @UnallocatedAmt   SET @UnallocatedAmt = 0.00					  END 
		if @AROAF = 0.00 set @AllocatedOAF = 0.00 else
		if @AROAF <= @UnallocatedAmt begin SET @allocatedOAF = @AROAF             SET @UnallocatedAmt = @UnallocatedAmt - @AROAF  END
									else begin SET @allocatedOAF = @UnallocatedAmt   SET @UnallocatedAmt = 0.00					  END 
		if @ARINT = 0.00 set @AllocatedINT = 0.00 else
		if @ARINT <= @UnallocatedAmt begin SET @allocatedINT = @ARINT             SET @UnallocatedAmt = @UnallocatedAmt - @ARINT  END
									else begin SET @allocatedINT = @UnallocatedAmt   SET @UnallocatedAmt = 0.00					  END 
		if @ARPD = 0.00 set @AllocatedPD = 0.00 else
		if @ARPD <= @UnallocatedAmt begin SET @allocatedPD = @ARPD             SET @UnallocatedAmt = @UnallocatedAmt - @ARPD  END
									else begin SET @allocatedPD = @UnallocatedAmt   SET @UnallocatedAmt = 0.00					  END 
    
		if @Debug = 1 select @allocatedFEES  '@allocatedFEES',@allocatedHARD	'@allocatedHARD',@allocatedSOFT	'@allocatedSOFT',@allocatedOAF	'@allocatedOAF'
							,@allocatedOAD	'@allocatedOAD',@allocatedINT	'@allocatedINT',@allocatedTAX	'@allocatedTAX',@allocatedPD	'@allocatedPD'
							,@allocatedRET 	'@allocatedRET' 

		if @UnallocatedAmt > 0.00  select 'Payment exceeds AR total'
		
		Set @AllocatedAmt = @allocatedFEES+@allocatedHARD+@allocatedSOFT+@allocatedOAF+@allocatedOAD+@allocatedINT+@allocatedTAX+@allocatedPD+@allocatedRET 
		if @AllocatedAmt <> @AmountToApply  select 'Error allocating completely AR total'
		
		declare @SourceTranUno integer, @currdate datetime, @NameUno integer, @Session integer, @Reference varchar(10)
		select @SourceTranUno = tran_uno
		     , @currdate = rec_status_date 
			 , @NameUno = NAME_UNO 
			 , @Session = [SESSION]
			 , @Reference = RECEIPT_REF 
		from crt_cash where receipt_num = @ReceiptNum

		if @Debug = 1 Select '[_spaac_Cr_Bill_Application]  Applied=' + isnull(convert(varchar(25),@AmountToApply),'NULL') 
						+ ', Allocated=' + isnull(convert(varchar(25),@AllocatedAmt), 'NULL') 
						+ ', UnAllocated=' + isnull(convert(varchar(25),@UnallocatedAmt ) ,'NULL') 

--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==
--++--++--==--==--++--++--==--==--++                 Do the database updates and inserts          ++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==
--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==
	
		
		select @SourceTranUno = tran_uno
		     , @currdate = rec_status_date 
		from crt_cash where receipt_num = @ReceiptNum

		if @Debug = 1 Select '[_spaac_Cr_Bill_Application]  cramt=' + isnull(convert(varchar(25),@CRAMT),'NULL') 
						+ ', Allocated=' + isnull(convert(varchar(25),@AllocatedAmt), 'NULL') 
						+ ', UnAllocated=' + isnull(convert(varchar(25),@UnallocatedAmt ) ,'NULL') 

		--Validate?:  Bill is posted and amount is outstanding

		--------------------------------------------------------  1. insert CR transaction row into BLT_BILL_AMT with allocted bucket amounts           1 1 1 1 1 1 1 1 1 1 1
		INSERT INTO BLT_BILL_AMT (INTERIM_WO,PAYR_CLIENT_UNO,MATTER_UNO,BILL_TRAN_UNO, MANUALLY_ALLOCATED,ASSIGN_EMPL_UNO
					,BILL_EMPL_UNO,RESP_EMPL_UNO,TC_CODE
					,TC_TOTAL_AMT
					,INTEREST_AMT
					,TAX_AMT
					,PREMDISC_AMT
					,RETAINER_AMT
					,OADISB_AMT
					,OAFEE_AMT
					,SOFT_AMT
					,HARD_AMT
					,FEES_AMT
					,DRUNO,WRITEOFF_BAL,SIGN,GL_AUDIT_NUM, PERIOD,TRAN_DATE,TRAN_TYPE
					,SOURCE_TRAN_UNO,BILLP_UNO,BILLM_UNO
					,ROW_UNO
					,LAST_MODIFIED) 
		select 'N',@PayorUno,@MatterUno,@btu,'N',0
					,m.bill_empl_uno,m.resp_empl_uno,'USD'
					,@AllocatedAmt
					,@allocatedINT
					,@allocatedTAX
					,@allocatedPD
					,@AllocatedRET
					,@AllocatedOAD
					,@AllocatedOAF
					,@AllocatedSoft
					,@allocatedHARD
					,@allocatedFEES
					,0,'N',-1,0,0, @currdate,'CR'
					,@SourceTranUno,bba.billp_uno,bba.billm_uno
					,next value for sequence.BLT_BILL_AMT  --ROW_UNO
					, getdate() 
		from BLT_BILL_AMT bba
		join hbm_matter m on bba.matter_uno = m.matter_uno
		where bba.matter_uno = @matteruno and bba.source_tran_uno = @BTU
		--^^^^^^^^^^^^^^^^

		--++++++++++++++++++++++++++++++++  Get remaining balance from BLT_BILL_AMT -- these will be the amounts to write off for allocation method 4  ================
		if @AllocationMethod = '4'  --  WO --  WO  --  WO --  WO  --  WO --  WO  --  WO --  WO  --  WO --  WO  --  WO --  WO  --  WO --  WO  --  WO --  WO  
		BEGIN 
			declare @BillDruno integer
		    declare @WOTranUno integer = next value for sequence.ACT_TRAN
			declare @TranPeriod integer
			select @TranPeriod = case when getdate() < cp1.cut_date then cur_per else year(@currdate) * 100 + month(@currdate) end 
				FROM CRM_PARMS CP1 join  GLM_PARMS GP1   on 1= 1  
			select 
			 @WOFEES = sum(sign * FEES_AMT)
			,@WOHARD = sum(sign * HARD_AMT)
			,@WOSOFT = sum(sign * SOFT_AMT)
			,@WOOAF	 = sum(sign * OAFEE_AMT)
			,@WOOAD	 = sum(sign * OADISB_AMT)
			,@WORET	 = sum(sign * RETAINER_AMT)
			,@WOPD	 = sum(sign * PREMDISC_AMT)
			,@WOTAX	 = sum(sign * TAX_AMT)
			,@WOINT	 = sum(sign * INTEREST_AMT)
			,@BillDruno = bl.DRUNO
			from blt_bill_amt  a
			join  blt_bill bl on a.bill_tran_uno = bl.tran_uno
			where  bill_tran_uno = @btu                                                                  and matter_uno = @MatterUno
			and a.druno >= -1
			group by bl.druno
			select @WO = @WOFEES + @WOHARD + @WOSOFT + @WOOAF + @WOOAD + @WORET + @WOPD	+ @WOTAX + @WOINT	
			if @WO <> 0.0
			BEGIN
				declare @WOText varchar(max) = @Reference + ' write off'
				declare @TextID integer
				exec sp_cmsnextkey_output 'ACT_TEXT',1, @TextID OUTPUT
				set @TextID = @Textid * 10
				INSERT INTO ACT_TEXT (TXT1,TXT2,TXT3,TXT4,TXT5,TXT6,SOURCE_UNO,COLUMN_ID,text_id, LAST_MODIFIED) 
				Select @WOText,'','','','','',@WOTranUno,0,@textid , getdate() 

				-- ACT_TRAN
				 INSERT INTO ACT_TRAN (TRANS_CURRENCY,SOURCE_OFFICE,TRANS_TEXT_ID, AUTHOR_EMPL_UNO,TRANS_CANCEL,
									  ORIG_TRAN_UNO,REASON_CODE,TRANS_PERIOD,TRANS_DATE, ENTRY_EMPL_UNO,
									  TRANS_NUMBER,TRANS_NAME_UNO,TRANS_SESSION,TRANS_TYPE,TRAN_UNO, LAST_MODIFIED, druno) 
				Select 
				'USD',@GLOffc,@TextID,@DefEmpl,'N',
				0,'92DC',@TranPeriod,@currdate,@DefEmpl,
				@BillNum,@nameuno, @session,'WO',@WOTranUno, getdate(), @BillDruno



				INSERT INTO BLT_BILL_AMT (INTERIM_WO,PAYR_CLIENT_UNO,MATTER_UNO,BILL_TRAN_UNO, MANUALLY_ALLOCATED,ASSIGN_EMPL_UNO
						,BILL_EMPL_UNO,RESP_EMPL_UNO,TC_CODE
						,TC_TOTAL_AMT
						,INTEREST_AMT
						,TAX_AMT
						,PREMDISC_AMT
						,RETAINER_AMT
						,OADISB_AMT
						,OAFEE_AMT
						,SOFT_AMT
						,HARD_AMT
						,FEES_AMT
						,DRUNO,WRITEOFF_BAL,SIGN,GL_AUDIT_NUM
						, PERIOD,TRAN_DATE,TRAN_TYPE
						,SOURCE_TRAN_UNO,BILLP_UNO,BILLM_UNO
						,ROW_UNO
						,LAST_MODIFIED) 
				select 'N',@PayorUno,@MatterUno,@btu,'N',0
						,m.bill_empl_uno,m.resp_empl_uno,'USD'
						,@WO
						,@WOINT
						,@WOTAX
						,@WOPD
						,@WORET
						,@WOOAD
						,@WOOAF
						,@WOSoft
						,@WOHARD
						,@WOFEES
						,@BillDruno,'N',-1,0
						,@TranPeriod, @currdate,'WO'
						,@WOTranUno -- Source Tran Uno for WO
						,bba.billp_uno,bba.billm_uno
						,next value for sequence.BLT_BILL_AMT  --ROW_UNO
						, getdate() 
				from BLT_BILL_AMT bba
				join hbm_matter m on bba.matter_uno = m.matter_uno
				where bba.matter_uno = @matteruno and bba.source_tran_uno = @BTU

				update TBH_MATTER_SUMM
				set last_modified = getdate(), LAST_AR_DATE = @CurrDate, LAST_AR_PER = @TranPeriod
				,WOFF_FEES      = WOFF_FEES     + @WOFEES       
				,WOFF_HARD		= WOFF_HARD		+ @WOHARD		
				,WOFF_INTEREST	= WOFF_INTEREST	+ @WOINT	
				,WOFF_OADISB	= WOFF_OADISB	+ @WOOAD	
				,WOFF_OAFEE		= WOFF_OAFEE	+ @WOOAF			
				,WOFF_PREMDIS	= WOFF_PREMDIS	+ @WOPD	
				,WOFF_RETNR		= WOFF_RETNR	+ @WORET		
				,WOFF_SOFT		= WOFF_SOFT		+ @WOSOFT		
				,WOFF_TAX		= WOFF_TAX		+ @WOTAX		
		
				,ARBAL_FEES		= ARBAL_FEES     - @WOFEES       
				,ARBAL_HARD		= ARBAL_HARD	 - @WOHARD		
				,ARBAL_INTEREST	= ARBAL_INTEREST - @WOINT	
				,ARBAL_OADISB	= ARBAL_OADISB	 - @WOOAD	
				,ARBAL_OAFEE	= ARBAL_OAFEE	 - @WOOAF			
				,ARBAL_PREMDIS	= ARBAL_PREMDIS	 - @WOPD	
				,ARBAL_RETNR	= ARBAL_RETNR	 - @WORET		
				,ARBAL_SOFT		= ARBAL_SOFT	 - @WOSOFT		
				,ARBAL_TAX		= ARBAL_TAX		 - @WOTAX	
				,ARBAL_TOTAL    = ARBAL_TOTAL    - @WO
				where matter_uno = @matteruno

				update TBH_CLIENT_SUMM
				set last_modified = getdate(), LAST_AR_DATE = @CurrDate, LAST_AR_PER = @TranPeriod
				,WOFF_FEES      = WOFF_FEES     + @WOFEES       
				,WOFF_HARD		= WOFF_HARD		+ @WOHARD		
				,WOFF_INTEREST	= WOFF_INTEREST	+ @WOINT	
				,WOFF_OADISB	= WOFF_OADISB	+ @WOOAD	
				,WOFF_OAFEE		= WOFF_OAFEE	+ @WOOAF			
				,WOFF_PREMDIS	= WOFF_PREMDIS	+ @WOPD	
				,WOFF_RETNR		= WOFF_RETNR	+ @WORET		
				,WOFF_SOFT		= WOFF_SOFT		+ @WOSOFT		
				,WOFF_TAX		= WOFF_TAX		+ @WOTAX		
		
				,ARBAL_FEES		= ARBAL_FEES     - @WOFEES       
				,ARBAL_HARD		= ARBAL_HARD	 - @WOHARD		
				,ARBAL_INTEREST	= ARBAL_INTEREST - @WOINT	
				,ARBAL_OADISB	= ARBAL_OADISB	 - @WOOAD	
				,ARBAL_OAFEE	= ARBAL_OAFEE	 - @WOOAF			
				,ARBAL_PREMDIS	= ARBAL_PREMDIS	 - @WOPD	
				,ARBAL_RETNR	= ARBAL_RETNR	 - @WORET		
				,ARBAL_SOFT		= ARBAL_SOFT	 - @WOSOFT		
				,ARBAL_TAX		= ARBAL_TAX		 - @WOTAX	
				,ARBAL_TOTAL    = ARBAL_TOTAL    - @WO
				where client_uno = (select client_uno from hbm_matter where matter_uno = @MatterUno)
			END -- If @WO <> 0.00

		END --@Allocation Method = 4
		else 
		BEGIN
			select 
			 @WOFEES = 0.00
			,@WOHARD = 0.00
			,@WOSOFT = 0.00
			,@WOOAF	 = 0.00
			,@WOOAD	 = 0.00
			,@WORET	 = 0.00
			,@WOPD	 = 0.00
			,@WOTAX	 = 0.00
			,@WOINT	 = 0.00
			select @WO = @WOFEES + @WOHARD + @WOSOFT + @WOOAF + @WOOAD + @WORET + @WOPD	+ @WOTAX + @WOINT	 
		END

	
		----------------------------------------------------------  2.	update BLT_BILLM to adjust AR amounts by subtracting paid amounts !! By Matter !!    2 2 2 2 2 2 2 2 2 
		if @Debug = 1 select 'At Step2: Matteruno:' + convert(varchar(25),@matteruno) + ', BTU:' + convert(varchar(25),@BTU)

		 Update BM Set LAST_MODIFIED = getdate()  
		 ,TOTAL_AR		= TOTAL_AR		- @AllocatedAmt  - @WO  
		 ,FEES_AR		= FEES_AR		- @allocatedFEES - @WOFEES 
		 ,HARD_AR		= HARD_AR		- @allocatedHARD - @WOHARD 
		 ,SOFT_AR		= SOFT_AR		- @AllocatedSoft - @WOSOFT 
		 ,OAFEE_AR		= OAFEE_AR		- @AllocatedOAF	 - @WOOAF	 
		 ,OADISB_AR		= OADISB_AR		- @AllocatedOAD	 - @WOOAD	 
		 ,RETAINER_AR	= RETAINER_AR	- @AllocatedRET	 - @WORET	 
		 ,PREMDISC_AR	= PREMDISC_AR	- @allocatedPD	 - @WOPD	 
		 ,TAX_AR		= TAX_AR		- @allocatedTAX	 - @WOTAX	 
		 ,INTEREST_AR	= INTEREST_AR	- @allocatedINT	 - @WOINT	
		 ,LATEST_DATE   = @currdate
		 ,LATEST_PERIOD = @tranPeriod
						from BLT_BILL_AMT bba
						join blt_billm bm on bba.billm_uno = bm.billm_uno
						join hbm_matter m on bba.matter_uno = m.matter_uno
						where bba.matter_uno = @matteruno and bba.source_tran_uno = @BTU

		----------------------------------------------------------  3.	update BLT_BILLP to adjust AR amounts by subtracting paid amounts !! By Payor !!    2 2 2 2 2 2 2 2 2 
		 Update BP Set LAST_MODIFIED = getdate()  
		 ,TOTAL_AR		= TOTAL_AR		- @AllocatedAmt   - @WO  
		 ,FEES_AR		= FEES_AR		- @allocatedFEES  - @WOFEES 
		 ,HARD_AR		= HARD_AR		- @allocatedHARD  - @WOHARD 
		 ,SOFT_AR		= SOFT_AR		- @AllocatedSoft  - @WOSOFT 
		 ,OAFEE_AR		= OAFEE_AR		- @AllocatedOAF	  - @WOOAF	
		 ,OADISB_AR		= OADISB_AR		- @AllocatedOAD	  - @WOOAD	
		 ,RETAINER_AR	= RETAINER_AR	- @AllocatedRET	  - @WORET	
		 ,PREMDISC_AR	= PREMDISC_AR	- @allocatedPD	  - @WOPD	
		 ,TAX_AR		= TAX_AR		- @allocatedTAX	  - @WOTAX	
		 ,INTEREST_AR	= INTEREST_AR	- @allocatedINT	  - @WOINT	
		  ,LATEST_DATE   = @currdate
		  ,LATEST_PERIOD = @tranPeriod
						from BLT_BILL_AMT bba
						join blt_billp bp on bba.billp_uno = bp.billp_uno
						join hbm_matter m on bba.matter_uno = m.matter_uno
						where bba.matter_uno = @matteruno and bba.source_tran_uno = @BTU

		--Calculate the percentage of billed fees being paid in this application
		DECLARE @HARDPCT NUMERIC(10,7) = case when @BLHArd = 0 then 1.0 else  @AllocatedHard / @BLHARD end
		DECLARE @SOFTPCT NUMERIC(10,7) = case when @BLSOFT = 0 then 1.0 else   @AllocatedSoft / @BLSOFT end
		DECLARE @FEESPCT NUMERIC(10,7) = case when @BLFEES = 0 then 1.0 else  @AllocatedFees / @BLFEES end
		if @Debug = 1 select 'Percents'
						,@AllocatedHard , @BLHARD ,@HARDPCT 
						,@AllocatedSoft , @BLSOFT ,@SOFTPCT
						,@AllocatedFees , @BLFEES ,@FEESPCT


		---------------------------------------------------   --  4S.	insert into BLH_PAID_DISB CR Trans (Credit Amount) by Cost_Code, Matter, BLT_BILLED_DISB UNO   4 4 4 4 4 4 4 4 4
		insert into blh_paid_disb ( row_uno,  cost_code, hardsoft, matter_uno, bill_tran_uno, source_tran_uno, tran_type, 
									period, gl_audit_num, credit_amount, billed_disb_uno, offc, dept, prof, druno, bal_zero_no_fca, last_modified)
		select next value for sequence.BLH_PAID_DISB, cost_code, hardsoft, matter_uno, bill_tran_uno, @SourceTranUno, 'CR', 
		  							0, 0, BILLED_AMT * @SOFTPCT , billed_disb_uno, offc, dept, prof, 0, NULL,    getdate()
		from BLH_BILLED_DISB where bill_tran_uno = @BTU and hardsoft = 'S'
		and matter_uno = @matteruno

		if @WOSOFT <> 0.00    --  WO --  WO  --  WO --  WO  --  WO --  WO  --  WO --  WO  --  WO --  WO  --  WO --  WO  --  WO --  WO  --  WO --  WO  
		BEGIN
			insert into blh_paid_disb ( row_uno,  cost_code, hardsoft, matter_uno, bill_tran_uno, source_tran_uno, tran_type, 
										period, gl_audit_num, credit_amount, billed_disb_uno, offc, dept, prof, druno, bal_zero_no_fca, last_modified)
			select next value for sequence.BLH_PAID_DISB, cost_code, hardsoft, matter_uno, bill_tran_uno, @WOTranUno, 'WO', 
		  								@TranPeriod, 0, BILLED_AMT * (1.0 - @SOFTPCT) , billed_disb_uno, offc, dept, prof, @BillDruno, NULL,    getdate()
			from BLH_BILLED_DISB where bill_tran_uno = @BTU and hardsoft = 'S'
			and matter_uno = @matteruno
		END



		---------------------------------------------------   --  5S.	update into BLH_BILLED_DISB CR Trans (PAID_CREDIT_TOT - allocated amount) by Cost_Code, Matter, BLT_BILLED_DISB UNO  5 5 5 5 5 5 5 5 5
		update bd set last_modified = getdate()
		,PAID_CREDIT_TOT = PAID_CREDIT_TOT - (BILLED_AMT * @SOFTPCT)
		from BLH_BILLED_DISB bd where bill_tran_uno = @BTU and hardsoft = 'S'
		and matter_uno = @matteruno
		
		if @WOSOFT <> 0.00 --  WO --  WO  --  WO --  WO  --  WO --  WO  --  WO --  WO  --  WO --  WO  --  WO --  WO  --  WO --  WO  --  WO --  WO  
		BEGIN 
			update bd set last_modified = getdate()
			,PAID_CREDIT_TOT = BILLED_AMT  -- Disbs should be fully paid (vs. (BILLED_AMT * (1.0 - @SOFTPCT) which could have rounding issues)
			from BLH_BILLED_DISB bd where bill_tran_uno = @BTU and hardsoft = 'S'
			and matter_uno = @matteruno
		END

		---------------------------------------------------   --  4H.	insert into BLH_PAID_DISB CR Trans (Credit Amount) by Cost_Code, Matter, BLT_BILLED_DISB UNO   4 4 4 4 4 4 4 4 4
		insert into blh_paid_disb ( row_uno,  cost_code, hardsoft, matter_uno, bill_tran_uno, source_tran_uno, tran_type, 
									period, gl_audit_num, credit_amount, billed_disb_uno, offc, dept, prof, druno, bal_zero_no_fca, last_modified)
		select next value for sequence.BLH_PAID_DISB, cost_code, hardsoft, matter_uno, bill_tran_uno, @SourceTranUno, 'CR', 
									0, 0, BILLED_AMT * @HARDPCT , billed_disb_uno, offc, dept, prof, 0, NULL,    getdate()
		from BLH_BILLED_DISB where bill_tran_uno = @BTU and hardsoft = 'H'
		and matter_uno = @matteruno

		if @WOHARD <> 0.00--  WO --  WO  --  WO --  WO  --  WO --  WO  --  WO --  WO  --  WO --  WO  --  WO --  WO  --  WO --  WO  --  WO --  WO  
		BEGIN
			insert into blh_paid_disb ( row_uno,  cost_code, hardsoft, matter_uno, bill_tran_uno, source_tran_uno, tran_type, 
										period, gl_audit_num, credit_amount, billed_disb_uno, offc, dept, prof, druno, bal_zero_no_fca, last_modified)
			select next value for sequence.BLH_PAID_DISB, cost_code, hardsoft, matter_uno, bill_tran_uno, @WOTranUno, 'WO', 
		  								@TranPeriod, 0, BILLED_AMT * (1.0 - @HARDPCT) , billed_disb_uno, offc, dept, prof, @BillDruno, NULL,    getdate()
			from BLH_BILLED_DISB where bill_tran_uno = @BTU and hardsoft = 'H'
			and matter_uno = @matteruno
		END

		---------------------------------------------------   --  5H.	update into BLH_BILLED_DISB CR Trans (PAID_CREDIT_TOT - allocated amount) by Cost_Code, Matter, BLT_BILLED_DISB UNO   5 5 5 5 5 5 5
		update bd set last_modified = getdate()
		,PAID_CREDIT_TOT = PAID_CREDIT_TOT - (BILLED_AMT * @HARDPCT)
		from BLH_BILLED_DISB bd where bill_tran_uno = @BTU and hardsoft = 'H'
		and matter_uno = @matteruno

		if @WOHARD <> 0.00--  WO --  WO  --  WO --  WO  --  WO --  WO  --  WO --  WO  --  WO --  WO  --  WO --  WO  --  WO --  WO  --  WO --  WO  
		BEGIN 
			update bd set last_modified = getdate()
			,PAID_CREDIT_TOT = BILLED_AMT  -- Disbs should be fully paid (vs. (BILLED_AMT * (1.0 - @HARDPCT) which could have rounding issues)
			from BLH_BILLED_DISB bd where bill_tran_uno = @BTU and hardsoft = 'H'
			and matter_uno = @matteruno
		END

		---------------------------------------------------   --  6.	insert into BLH_PAID_FEES CR Trans (Credit Amount) by Cost_Code, Matter, BLT_BILLED_FEES UNO        6 6 6 6 6 6 6 6 6 6
		insert into blh_paid_fees ( row_uno,  tk_empl_uno, matter_uno, bill_tran_uno, source_tran_uno, tran_type, 
						cr_empl_uno, period, gl_audit_num, credit_amount, billed_fees_uno, offc, dept, prof, from_premdisc, druno, bal_zero_no_fca, last_modified)
		select next value for sequence.BLH_PAID_FEES, tk_empl_uno, matter_uno, bill_tran_uno, @SourceTranUno, 'CR', 
						cr_empl_uno, 0, 0, BILLED_AMT * @FEESPCT, billed_fees_uno, offc, dept, prof, 'N',   0, NULL,    getdate()
		from BLH_BILLED_FEES where bill_tran_uno = @BTU 
		and matter_uno = @matteruno

		if @WOFEES <> 0.00--  WO --  WO  --  WO --  WO  --  WO --  WO  --  WO --  WO  --  WO --  WO  --  WO --  WO  --  WO --  WO  --  WO --  WO  
		BEGIN
			insert into blh_paid_fees ( row_uno,  tk_empl_uno, matter_uno, bill_tran_uno, source_tran_uno, tran_type, 
						cr_empl_uno, period, gl_audit_num, credit_amount, billed_fees_uno, offc, dept, prof, from_premdisc, druno, bal_zero_no_fca, last_modified)
		select next value for sequence.BLH_PAID_FEES, tk_empl_uno, matter_uno, bill_tran_uno, @WOTranUno, 'WO', 
						cr_empl_uno, @TranPeriod , 0, BILLED_AMT * (1.0 - @FEESPCT), billed_fees_uno, offc, dept, prof, 'N',   @BillDruno , NULL,    getdate()
		from BLH_BILLED_FEES where bill_tran_uno = @BTU 
		and matter_uno = @matteruno
		END

		---------------------------------------------------   -- 7.	update into BLH_BILLED_FEES CR Trans (PAID_CREDIT_TOT - allocated amount) by Cost_Code, Matter, BLT_BILLED_FEES UNO   7 7 7 7 7 7 7 7 7
		update bf set last_modified = getdate()
		,PAID_CREDIT_TOT = PAID_CREDIT_TOT - (BILLED_AMT * @FEESPCT)
		from BLH_BILLED_FEES bf where bill_tran_uno = @BTU 
		and matter_uno = @matteruno

		if @WOFEES <> 0.00--  WO --  WO  --  WO --  WO  --  WO --  WO  --  WO --  WO  --  WO --  WO  --  WO --  WO  --  WO --  WO  --  WO --  WO  
		BEGIN
			update bf set last_modified = getdate()
			,PAID_CREDIT_TOT = BILLED_AMT -- PAID_CREDIT_TOT - (BILLED_AMT * @FEESPCT)
			from BLH_BILLED_FEES bf where bill_tran_uno = @BTU 
			and matter_uno = @matteruno
		END




	
	END  --@AmountToApply <> zero



--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==
--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==--++--++--==--==
END -- Matter loop
close BM
deallocate BM
Declare @ShortPayToAllocate numeric(17,10) = 0.0
if @UnallocatedCRAmt < 0 -- This means we applied more than the AR was.  This is possible for method 3 where diffrenece should go to bank fees
   if @AllocationMethod in ('3')
   BEGIN -- Add allocation to bank fees
		select @UnallocatedCRAmt 'Amt for Bank Fees or Writeoff'
		Set @ShortPayToAllocate = @UnallocatedCRAmt
	END -- Bank Fees
	else
		select @UnallocatedCRAmt 'Amt for Matter UAC1'
if @UnallocatedCRAmt > 0 
		select @UnallocatedCRAmt 'Amt for Matter UAC2'

---------------------------------------------------   -- 8a.	Create tran for bank charges if allocationmethod = 3       8  8  8  8  8  8  8 
	declare @Text varchar(max) 
	if @ShortPayToAllocate <> 0 and @AllocationMethod = '3'
	BEGIN
		declare @GLTextUno integer, @GLRowUno integer
		exec SP_CMSNEXTKEY_OUTPUT 'GLT_TEXT',1, @GLTextUno OUTPUT
		exec SP_CMSNEXTKEY_OUTPUT 'ACT_TRAN_JE',1, @GLRowUno OUTPUT
		set @Text = 'Bank Fees; Import Batch:' + convert(varchar(10), @Batchid) + '; TranRef:' + @TranRef 
		INSERT INTO ACT_TEXT (TXT1,TXT2,TXT3,TXT4,TXT5,TXT6,SOURCE_UNO,COLUMN_ID,TEXT_ID, LAST_MODIFIED) 
		VALUES (@Text,'','','','','',@SourceTranUno ,0,@GLTextUno, getdate() )

		INSERT INTO ACT_TRAN_JE (
		TC_CODE,TC_DEBIT,TC_CREDIT,PROJ,TRAN_TYPE,TRAN_UNO
		,GL_ROW_UNO,ACCT_UNO,OFFC,DEPT,PROF,EMPL_UNO
		,GL_TEXT_ID,DEBIT,CREDIT,DRUNO,OFFC_INTERBOOK, LAST_MODIFIED) 
		VALUES ('USD',0.0 - @ShortPayToAllocate,0.0,'','CR',@SourceTranUno
		,@GLRowUno,@BankAcctUno,@GLOffc,@DefDept,@DefProf,@DefEmpl
		,@GLTextUno,  0.0 - @ShortPayToAllocate,0.0,  0, @GLOffc, getdate() )
	END  --Bank Charges Entry
-- 8a END -------------------------------------------------------------
---------------------------------------------------   -- 8b.	Create tran for bank charges if allocationmethod = 3       8  8  8  8  8  8  8 
	--Managed inline above
	--if @ShortPayToAllocate <> 0 and @AllocationMethod = '4'
	--BEGIN
	--	select 'TBD'
	--END  --Bank Charges Entry

-- 8b END -------------------------------------------------------------



	if @UnallocatedCRAmt > 0 -- Create UAC credit on general matter
	BEGIN
		Declare @CreditMatter integer = (select matter_uno from hbm_matter where client_uno = @PayorUno and matter_code = '0001001')
		Declare @ATCMA_ROW_UNO integer 
		exec SP_CMSNEXTKEY_OUTPUT 'ACT_TRAN_CR_MA',1,@ATCMA_ROW_UNO OUTPUT
		set @Text = 'Matter OverPmt; Import Batch:' + convert(varchar(10), @Batchid) + '; TranRef:' + @TranRef 
		INSERT INTO ACT_TRAN_CR_MA (
		TC_CODE,TC_AMOUNT,TRAN_UNO,TRAN_DATE,TRAN_TYPE,BILL_NUM
		,SIGN,GL_AUDIT_NUM,PERIOD,EXPIRY_DATE
		,REMARKS,DRUNO,AMOUNT,DATA_TYPE,Matter_uno,ROW_UNO, LAST_MODIFIED
		) 
		VALUES ('USD'             --TC_CODE
				,@UnallocatedCRAmt             --TC_AMOUNT
				,@SourceTranUno              --TRAN_UNO
				,@currdate     --TRAN_DATE
				,'CR'             --TRAN_TYPE
				,0                --BILL_NUM
				,1                --SIGN
				,0                --GL_AUDIT_NUM
				,0                --PERIOD,
				,NULL             --XPIRY_DATE
				,@Text   --REMARKS
				,0                --DRUNO
				,@UnallocatedCRAmt             --AMOUNT
				,'C'              --DATA_TYPE
				,@CreditMatter                --Matter UNO
				,@ATCMA_ROW_UNO              --ROW_UNO
				,getdate() -- LAST_MODIFIED
				)     

	END  -- apply overpmt to matter credit

If @ThisIsMyTransaction = 1
	BEGIN
		Commit transaction
	END



GO

/****** Object:  StoredProcedure [dbo].[_spaac_Cr_Create_Receip]    Script Date: 1/22/2023 8:49:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE proc [dbo].[_spaac_Cr_Create_Receip]
	 @BatchID Integer
	,@Session Integer
	,@TranSeq Integer
	,@ReceiptNum Integer  OUTPUT
AS


        
DECLARE @TranAmt money --! 
DECLARE @billnumString varchar(max) = '' 
DECLARE @CheckNum char(15)
DECLARE @checkdate datetime
DECLARE @DepDate datetime
DECLARE @reference char(20)
DECLARE @DrawnBy varchar(40)
Declare @ReceiptType varchar(5)

DECLARE @PayorName varchar(40) 
DECLARE @PayorCode varchar(10) 
declare @ReceiptComment varchar(max) = ''
declare @AddnlComments varchar(max) = ''
SELECT
 @TranAmt        = r.amt
,@billnumString  = @billnumString +  ', ' + d.invoiceno
,@CheckNum 		 = isnull(r.CheckNUm,'')
,@checkdate		 = r.ReceiptDate
,@DepDate 		 = r.ReceiptDate
,@reference		 = r.TranRef
,@DrawnBy 		 = r.DrawnBy 
,@PayorName		 = r.DrawnBy 
,@PayorCode		 = r.payor
,@ReceiptComment = r.DistDescription 
,@ReceiptType	 = r.ReceiptType
From _aac_cr_load_b d
join _aac_cr_load_b R on d.transeq = r.transeq and r.applicationtype = 'R'
where d.applicationtype <> 'R' and d.batchID = @BatchID and d.TranSeq = @TranSeq and d.batchid = r.batchid
select @billnumString = substring(@billnumString, 3,250)


declare @DefEmpl integer 
declare @DefDept varchar(10) 
declare @DefProf varchar(10)
declare @GLOffc varchar(10)
select  @DefEmpl = OVH_EMPL_UNO
       ,@DefDept = OVH_DEPT 
       ,@DefProf = OVH_PROF 
       ,@GLOffc  = DEF_OFFC      from glm_parms


set @AddnlComments = 'PayorName:' + isnull(@payorname,'Unknown') + isnull(' (' + @PayorCode + ')','')

select @AddnlComments = @AddnlComments + '; ' + ErrorValue from _aac_cr_load_b a 
left join _AacCRImpErrorLog b on a.lineseq = b.TrackingNumber AND A.BATCHID = B.BatchID 
WHERE A.batchid = @BatchID  AND A.TRANSEQ = @TranSeq and isnull(ErrorCode,'') <> ''


--07/08 - mpc - Updated so if the number of payors <> 1, then make the payor 'Unknown'

--create Values from Session
declare @bank char(6)
declare @CurrDate datetime
select @bank = ses_bank, @currDate = create_date from crs_cash_z where session = @session

declare @PayerClientUno integer = -1
declare @nameuno integer
declare @FirstBill varchar(15)

if charindex(',',@billnumstring) = 0
    select @firstbill = @billnumstring
else
    select @firstbill = left(@billnumstring,charindex(',',@billnumstring)-1)
-- Payor should be the payor from the R row.
select @PayerClientUno = client_uno from hbm_client where client_code = @PayorCode
-- If no payor on header, try to get payor from bill.
if @PayerClientUno = -1
BEGIN
select @PayerClientUno = payr_client_uno 
     from blt_billp a join blt_bill b on a.bill_tran_uno = b.tran_uno and b.bill_num = @firstbill and @firstbill <> ''
     order by total_ar desc
END

if @PayerClientUno = -1
    BEGIN
       select @payerClientUno = 0, @nameuno = 0, @payorName='Unknown', @payorcode = 'NA'
    END
ELSE
    BEGIN
        select @nameuno = name_uno, @payorName = isnull(client_name,'UnKnown'), @payorcode = isnull(client_code,'NA')
          from hbm_client where client_uno = @PayerClientUno
    END
Declare @ThisIsMyTransaction as bit = 0
If @@Trancount = 0
BEGIN
	begin transaction
	Set @THisIsMyTransaction = 1
	Select '[_spaac_Cr_Create_Receip] begins transaction'
END
set @ReceiptNum = -1
DECLARE @TRANUNO integer = -1
DECLARE @textid integer = -1


EXEC sp_cmsnextkey_output 'ACT_TRAN',1,@tranuno OUTPUT
EXEC sp_cmsnextkey_output 'CRT_TEXT',1,@textid OUTPUT
EXEC sp_cmsnextkey_output 'CRT_CASH.RECEIPT_NUM',1,@ReceiptNum OUTPUT
--receipt num goes in tran number
            INSERT INTO ACT_TRAN (TRANS_CURRENCY,SOURCE_OFFICE,TRANS_TEXT_ID, AUTHOR_EMPL_UNO,TRANS_CANCEL,
                                  ORIG_TRAN_UNO,REASON_CODE,TRANS_PERIOD,TRANS_DATE, ENTRY_EMPL_UNO,
                                  TRANS_NUMBER,TRANS_NAME_UNO,TRANS_SESSION,TRANS_TYPE,TRAN_UNO, LAST_MODIFIED, druno) 
            Select 
			'USD',@GLOffc,0,@DefEmpl,'N',
			0,'',0,@currdate,@DefEmpl,
			@ReceiptNum,@nameuno, @session,'CR',@tranuno, getdate(), 0 

            INSERT INTO CRT_CASH (TC_CODE,TC_RECEIPT_AMT,RECEIPT_STATUS,REC_STATUS_DATE, PERIOD,GL_AUDIT_NUM,
                                  REIMBURSEMENT,ORIGINAL_AMT,REVERSAL,IMAGE_ID,MATTER_UNO, BILL_NUMBERS,RECEIPT_TYPE,
                                  DRAWN_BY,PAYOR_BANK,SESSION,PAYOR_TYPE,TEXT_ID, NAME_UNO,RECEIPT_REF,BANK_CODE,
                                  CHECK_NUM,CURRENCY_CODE,DEPOSIT_DATE,CLIENT_UNO, EMPL_UNO,VENDOR_UNO,TRAN_UNO,
                                  RECEIPT_NUM,CHECK_DATE,DRUNO,RECEIPT_AMT, IMAGE_FILES,SPOT_RATE, LAST_MODIFIED) 
            SELECT 
			'USD',@TranAmt,'N',@CurrDate,0,0,where 
                    'N',@TranAmt,'N',0,0,@billnumString 'BillNumString',@ReceiptType,
                    @DrawnBy,'',@session,'C',@textid * 10,@nameuno,@reference,@bank,     
                    @checknum,'USD',@DepDate,@PayerClientUno,0,0,@TranUno,
                    @ReceiptNum,@CheckDate,0,@TranAMT,'',0, getdate()  



            INSERT INTO CRT_TEXT (TXT1,TXT2,TXT3,TXT4,TXT5,TXT6,SOURCE_UNO,COLUMN_ID,text_id, LAST_MODIFIED) 
            Select @ReceiptComment,'','','','','',@TranUno,0,@textid * 10, getdate() 
If @ThisIsMyTransaction = 1
BEGIN
	commit 
	Select '[_spaac_Cr_Create_Receip] Commits'
END
GO

/****** Object:  StoredProcedure [dbo].[_spaac_MT_Credit_Application]    Script Date: 1/22/2023 8:49:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE   proc [dbo].[_spaac_MT_Credit_Application]
		 @ReceiptNum	integer
		,@BatchID		integer
		,@TranSeq		integer
		,@TranLineSeq	integer
		,@AmtApplied	money  OUTPUT
AS
--Generate UNOS
declare @TranUno integer
declare @ATCMA_ROWUNO integer
exec sp_cmsnextkey_output 'ACT_TRAN', 1, @TranUno OUTPUT
exec sp_cmsnextkey_output 'ACT_TRAN_CR_MA', 1, @ATCMA_ROWUNO OUTPUT


declare @MatterUno integer

declare @OFFC        varchar(5)  
declare @CRDate    date
declare @EmplUno     integer
declare @Session     integer -- Get from @ReceiptNum   
declare @SourceTranUno integer
declare @CRAmount     money 
declare @ReceiptRef   varchar(20)
declare @Bank         varchar(5)
declare @Remarks  varchar(200)

 select 
 @MatterUno = m.matter_uno
,@CRDate = ReceiptDate
,@CRAmount = convert(numeric(17,4), amt)
,@Session = crc.session
,@ReceiptRef = cr.TranRef
,@SourceTranUno = tran_uno
,@Remarks = DistDescription
 from _aac_cr_load_b cr
 left join hbm_matter m on cr.payor = m.client_code and cr.MatterID = m.matter_code
 left join crt_cash crc on crc.receipt_num = @ReceiptNUm
 where applicationtype = 'M' and transeq = @TranSeq and batchid = @batchid and TranLineSeq =  @TranLineSeq   
 
 select
  MatterUno = @MatterUno 
 ,CRDate    = @CRDate 
 ,CRAmount  = @CRAmount 
 ,Session   = @Session 
 ,ReceiptRef = @ReceiptRef 


--INSERT INTO ACT_TRAN (
--TRANS_CURRENCY
--,SOURCE_OFFICE
--,TRANS_TEXT_ID
--,AUTHOR_EMPL_UNO
--,TRANS_CANCEL
--,ORIG_TRAN_UNO
--,REASON_CODE
--,TRANS_PERIOD
--,TRANS_DATE
--,ENTRY_EMPL_UNO
--,TRANS_NUMBER
--,TRANS_NAME_UNO
--,TRANS_SESSION
--,TRANS_TYPE
--,TRAN_UNO
--,LAST_MODIFIED
--)
--VALUES (

--'USD'        --TRANS_CURRENCY
--,@OFFC       --SOURCE_OFFICE
--,0           --TRANS_TEXT_ID
--,0           --AUTHOR_EMPL_UNO
--,'N'         --TRANS_CANCEL
--,0           --ORIG_TRAN_UNO
--,''          --REASON_CODE
--,0           --TRANS_PERIOD
--,@CRDate --TRANS_DATE 
--,@EmplUno            --ENTRY_EMPL_UNO
--,@ReceiptNum       --TRANS_NUMBER - SET ReceiptNum
--,0            --TRANS_NAME_UNO
--,@Session         --TRANS_SESSION  CR Session
--,'CR'         --TRANS_TYPE
--,@TranUno        --TRAN_UNO
--, getdate() --LAST_MODIFIED
--) 

---- CRT_CASH should be created separate earlier in the process when we create the session
	----INSERT INTO CRT_CASH (
	----TC_CODE
	----,TC_RECEIPT_AMT
	----,RECEIPT_STATUS
	----,REC_STATUS_DATE
	----,PERIOD
	----,GL_AUDIT_NUM
	----,REIMBURSEMENT
	----,ORIGINAL_AMT
	----,REVERSAL
	----,IMAGE_ID
	----,MATTER_UNO
	----,BILL_NUMBERS
	----,RECEIPT_TYPE
	----,DRAWN_BY
	----,PAYOR_BANK
	----,SESSION
	----,PAYOR_TYPE
	----,TEXT_ID
	----,NAME_UNO
	----,RECEIPT_REF
	----,BANK_CODE
	----,CHECK_NUM
	----,CURRENCY_CODE
	----,DEPOSIT_DATE
	----,CLIENT_UNO
	----,EMPL_UNO
	----,VENDOR_UNO
	----,TRAN_UNO
	----,RECEIPT_NUM
	----,CHECK_DATE
	----,DRUNO
	----,RECEIPT_AMT
	----,IMAGE_FILES
	----,SPOT_RATE
	----,EI_ID
	----,RECEIVED_DATE
	----, LAST_MODIFIED
	----) VALUES (
	----'USD'          --TC_CODE
	----,@CRAmount           --TC_RECEIPT_AMT
	----,'N'           --RECEIPT_STATUS
	----,@CRDate  --REC_STATUS_DATE
	----,0             --PERIOD
	----,0             --GL_AUDIT_NUM
	----,'N'           --REIMBURSEMENT
	----,@CRDate             --ORIGINAL_AMT
	----,'N'           --REVERSAL
	----,0             --IMAGE_ID
	----,0             --MATTER_UNO
	----,''            --BILL_NUMBERS
	----,'C'           --RECEIPT_TYPE
	----,''            --DRAWN_BY
	----,''            --PAYOR_BANK
	----,@Session           --SESSION
	----,'C'           --PAYOR_TYPE
	----,0             --TEXT_ID
	----,0             --NAME_UNO
	----,@ReceiptRef         --RECEIPT_REF
	----,@Bank         --BANK_CODE
	----,''            --CHECK_NUM
	----,'USD'         --CURRENCY_CODE
	----,@CRDate  --DEPOSIT_DATE
	----,0             --CLIENT_UNO
	----,0             --EMPL_UNO
	----,0             --VENDOR_UNO
	----,@TranUno         --TRAN_UNO
	----,@ReceiptNum         --RECEIPT_NUM
	----,NULL          --CHECK_DATE
	----,0             --DRUNO
	----,@CRAmount           --RECEIPT_AMT
	----,''            --IMAGE_FILES
	----,0             --SPOT_RATE
	----,''            --EI_ID
	----,NULL          --RECEIVED_DATE
	----, getdate() )  -- LAST_MODIFIED
INSERT INTO ACT_TRAN_CR_MA 
(TC_CODE
,TC_AMOUNT
,TRAN_UNO
,TRAN_DATE
,TRAN_TYPE
,BILL_NUM
,SIGN
,GL_AUDIT_NUM
,PERIOD,
EXPIRY_DATE
,REMARKS
,DRUNO
,AMOUNT
,DATA_TYPE
,Matter_uno
,ROW_UNO
, LAST_MODIFIED
) 
VALUES (  

'USD'             --TC_CODE
,@CRAmount             --TC_AMOUNT
,@SourceTranUno              --TRAN_UNO
,@CRDate     --TRAN_DATE
,'CR'             --TRAN_TYPE
,0                --BILL_NUM
,1                --SIGN
,0                --GL_AUDIT_NUM
,0                --PERIOD,
,NULL             --XPIRY_DATE
,@Remarks   --REMARKS
,0                --DRUNO
,@CRAmount             --AMOUNT
,'C'              --DATA_TYPE
,@MatterUno                --CLIENT_UNO
,@ATCMA_ROWUNO              --ROW_UNO
,getdate() -- LAST_MODIFIED
)     
------SELECT TOP 1 * FROM CRS_CASH_Z ORDER BY SESSION DESC
------SELECT TOP 1 * FROM CRT_CASH ORDER BY SESSION DESC
------SELECT TOP 1 * FROM ACT_TRAN_CR_CL ORDER BY ROW_UNO DESC      
select @amtapplied = @CRAmount
GO

/****** Object:  StoredProcedure [dbo].[_spaac_CL_Credit_Application]    Script Date: 1/22/2023 8:49:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO







CREATE   proc [dbo].[_spaac_CL_Credit_Application]
		 @ReceiptNum	integer
		,@BatchID		integer
		,@TranSeq		integer
		,@TranLineSeq	integer
		,@AmtApplied	money  OUTPUT
AS
--Generate UNOS
declare @TranUno integer
declare @ATCMA_ROWUNO integer
exec sp_cmsnextkey_output 'ACT_TRAN', 1, @TranUno OUTPUT
exec sp_cmsnextkey_output 'ACT_TRAN_CR_CL', 1, @ATCMA_ROWUNO OUTPUT


declare @ClientUno integer

declare @OFFC        varchar(5)  
declare @CRDate    date
declare @EmplUno     integer
declare @Session     integer -- Get from @ReceiptNum   
declare @SourceTranUno integer
declare @CRAmount     money 
declare @ReceiptRef   varchar(20)
declare @Bank         varchar(5)
declare @Remarks varchar(200)

 select 
 @clientUno = c.client_uno
,@CRDate = ReceiptDate
,@CRAmount = convert(numeric(17,4), amt)
,@Session = crc.session
,@ReceiptRef = cr.TranRef
,@SourceTranUno = tran_uno
,@Remarks = DistDescription
 from _aac_cr_load_b cr
 left join hbm_client c on cr.payor = c.client_code
 left join crt_cash crc on crc.receipt_num = @ReceiptNUm
 where applicationtype = 'c' and transeq = @TranSeq and batchid = @batchid    and TranLineSeq = @TranLineSeq
 
           


 select
  clientUno = @clientUno 
 ,CRDate    = @CRDate 
 ,CRAmount  = @CRAmount 
 ,Session   = @Session 
 ,ReceiptRef = @ReceiptRef 


------INSERT INTO ACT_TRAN (
------TRANS_CURRENCY
------,SOURCE_OFFICE
------,TRANS_TEXT_ID
------,AUTHOR_EMPL_UNO
------,TRANS_CANCEL
------,ORIG_TRAN_UNO
------,REASON_CODE
------,TRANS_PERIOD
------,TRANS_DATE
------,ENTRY_EMPL_UNO
------,TRANS_NUMBER
------,TRANS_NAME_UNO
------,TRANS_SESSION
------,TRANS_TYPE
------,TRAN_UNO
------,LAST_MODIFIED
------)
------VALUES (

------'USD'        --TRANS_CURRENCY
------,@OFFC       --SOURCE_OFFICE
------,0           --TRANS_TEXT_ID
------,0           --AUTHOR_EMPL_UNO
------,'N'         --TRANS_CANCEL
------,0           --ORIG_TRAN_UNO
------,''          --REASON_CODE
------,0           --TRANS_PERIOD
------,@CRDate --TRANS_DATE 
------,@EmplUno            --ENTRY_EMPL_UNO
------,@ReceiptNum       --TRANS_NUMBER - SET ReceiptNum
------,0            --TRANS_NAME_UNO
------,@Session         --TRANS_SESSION  CR Session
------,'CR'         --TRANS_TYPE
------,@TranUno        --TRAN_UNO
------, getdate() --LAST_MODIFIED
------) 

---- CRT_CASH should be created separate earlier in the process when we create the session
	----INSERT INTO CRT_CASH (
	----TC_CODE
	----,TC_RECEIPT_AMT
	----,RECEIPT_STATUS
	----,REC_STATUS_DATE
	----,PERIOD
	----,GL_AUDIT_NUM
	----,REIMBURSEMENT
	----,ORIGINAL_AMT
	----,REVERSAL
	----,IMAGE_ID
	----,MATTER_UNO
	----,BILL_NUMBERS
	----,RECEIPT_TYPE
	----,DRAWN_BY
	----,PAYOR_BANK
	----,SESSION
	----,PAYOR_TYPE
	----,TEXT_ID
	----,NAME_UNO
	----,RECEIPT_REF
	----,BANK_CODE
	----,CHECK_NUM
	----,CURRENCY_CODE
	----,DEPOSIT_DATE
	----,CLIENT_UNO
	----,EMPL_UNO
	----,VENDOR_UNO
	----,TRAN_UNO
	----,RECEIPT_NUM
	----,CHECK_DATE
	----,DRUNO
	----,RECEIPT_AMT
	----,IMAGE_FILES
	----,SPOT_RATE
	----,EI_ID
	----,RECEIVED_DATE
	----, LAST_MODIFIED
	----) VALUES (
	----'USD'          --TC_CODE
	----,@CRAmount           --TC_RECEIPT_AMT
	----,'N'           --RECEIPT_STATUS
	----,@CRDate  --REC_STATUS_DATE
	----,0             --PERIOD
	----,0             --GL_AUDIT_NUM
	----,'N'           --REIMBURSEMENT
	----,@CRDate             --ORIGINAL_AMT
	----,'N'           --REVERSAL
	----,0             --IMAGE_ID
	----,0             --MATTER_UNO
	----,''            --BILL_NUMBERS
	----,'C'           --RECEIPT_TYPE
	----,''            --DRAWN_BY
	----,''            --PAYOR_BANK
	----,@Session           --SESSION
	----,'C'           --PAYOR_TYPE
	----,0             --TEXT_ID
	----,0             --NAME_UNO
	----,@ReceiptRef         --RECEIPT_REF
	----,@Bank         --BANK_CODE
	----,''            --CHECK_NUM
	----,'USD'         --CURRENCY_CODE
	----,@CRDate  --DEPOSIT_DATE
	----,0             --CLIENT_UNO
	----,0             --EMPL_UNO
	----,0             --VENDOR_UNO
	----,@TranUno         --TRAN_UNO
	----,@ReceiptNum         --RECEIPT_NUM
	----,NULL          --CHECK_DATE
	----,0             --DRUNO
	----,@CRAmount           --RECEIPT_AMT
	----,''            --IMAGE_FILES
	----,0             --SPOT_RATE
	----,''            --EI_ID
	----,NULL          --RECEIVED_DATE
	----, getdate() )  -- LAST_MODIFIED
INSERT INTO ACT_TRAN_CR_CL 
(TC_CODE
,TC_AMOUNT
,TRAN_UNO
,TRAN_DATE
,TRAN_TYPE
,BILL_NUM
,SIGN
,GL_AUDIT_NUM
,PERIOD,
EXPIRY_DATE
,REMARKS
,DRUNO
,AMOUNT
,DATA_TYPE
,CLIENT_UNO
,ROW_UNO
, LAST_MODIFIED
) 
VALUES (  

'USD'             --TC_CODE
,@CRAmount             --TC_AMOUNT
,@SourceTranUno             --TRAN_UNO
,@CRDate     --TRAN_DATE
,'CR'             --TRAN_TYPE
,0                --BILL_NUM
,1                --SIGN
,0                --GL_AUDIT_NUM
,0                --PERIOD,
,NULL             --XPIRY_DATE
,@Remarks  --REMARKS
,0                --DRUNO
,@CRAmount             --AMOUNT
,'C'              --DATA_TYPE
,@ClientUno                --CLIENT_UNO
,@ATCMA_ROWUNO              --ROW_UNO
,getdate() -- LAST_MODIFIED
)     
------SELECT TOP 1 * FROM CRS_CASH_Z ORDER BY SESSION DESC
------SELECT TOP 1 * FROM CRT_CASH ORDER BY SESSION DESC
------SELECT TOP 1 * FROM ACT_TRAN_CR_CL ORDER BY ROW_UNO DESC      
select @amtapplied = @CRAmount
GO

/****** Object:  StoredProcedure [dbo].[_spaac_Cr_Create_Session]    Script Date: 1/22/2023 8:49:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE proc [dbo].[_spaac_Cr_Create_Session] @BatchID Integer, @transeqfilter integer = -1,  @session integer OUTPUT 
AS
-- sp_help crs_cash_z
declare @SessionDate datetime
declare @bank char(6)
declare @SessionTotal money
declare @Reference char(20)
declare @DefEmpl integer = (select OVH_EMPL_UNO from glm_parms)
select 
 @SessionDate	=  ReceiptDate
,@Bank			= BankCode 
,@Reference		=  case when _sourcefile like '%\%' 
					then reverse(left(reverse(_sourcefile),charindex('\',reverse(_sourcefile))-1))
					else right(_sourcefile,20)
					end
,@SessionTotal	= sum(convert(numeric(17,4),amt))

from _aac_cr_load_b
where BatchID =  @BatchID and applicationtype = 'R' and (transeq = @transeqfilter or @transeqfilter = -1)
group by  ReceiptDate,  BankCode,  
  case when _sourcefile like '%\%' 
					then reverse(left(reverse(_sourcefile),charindex('\',reverse(_sourcefile))-1))
					else right(_sourcefile,20)
					end

-----Create Session Header
declare @currdate datetime
exec sp_cmsnextkey_output 'CRS_CASH_Z',1,@session OUTPUT
--set @session = -1
select @currdate = convert(date,getdate())

            INSERT INTO CRS_CASH_Z (STATUS_EMPL_UNO,CREATE_EMPL_UNO,SOURCE_REF,           
                                    STATUS_DATE,PRINTED,SES_DATE,SES_BANK,CONTROL,SESSION,
                                    SOURCE,CREATE_DATE,STATUS,RECEIPT_REF, LAST_MODIFIED) 
            VALUES (
			 @DefEmpl,@DefEmpl,0,@currdate,'N',@sessiondate, @bank,@sessiontotal,@session,'MA',@currdate,'E',@Reference, getdate() 
			)   
select '[_spaac_Cr_Create_Session] Created new session ' + isnull(convert(varchar(17),@session),'NULL')
GO

/****** Object:  StoredProcedure [dbo].[_spaac_CR_Import_Map]    Script Date: 1/22/2023 8:49:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



Create proc [dbo].[_spaac_CR_Import_Map] (@JSON nvarchar(max))
as
declare @MapCode varchar(10)
select @MapCode = MapCode 
FROM OPENJSON(@json) WITH (   
 MapCode     varchar(15) '$.MapCode'
) as z

begin transaction
--set identity_insert _AAC_CRMAPZ  on 
--set identity_insert _AAC_CRMAP  on 
delete from _AAC_CRMAP where mapcode = @MapCode
delete from _AAC_CRMAPZ where mapcode = @MapCode

;with mapz as (SELECT 
 z.MapCode
,z.MapDesc
,z.FileMask
,z.filetype
,z.inactive
,z.receiptID
,z.TargetTable
,z.XLSheetName

FROM OPENJSON(@json) WITH (   
 MapCode     varchar(15) '$.MapCode'
,MapDesc     varchar(15) '$.MapDesc'
,FileMask	 varchar(15) '$.FileMask'
,FileType	 varchar(15) '$.FileType'
,Inactive	 varchar(15) '$.Inactive'
,ReceiptID	 varchar(15) '$.ReceiptID'
,TargetTable varchar(15) '$.TargetTable'
,XLSheetName varchar(15) '$.XLSheetName'
) AS z
)
insert into _AAC_CRMAPZ (MapCode	,MapDesc	,FileMask	,FileType	,Inactive	,ReceiptID	,TargetTable	,XLSheetName) select * from mapz

select * from _aac_crmapz where mapcode = @MapCode


;with map as (SELECT 
 m.MapCode
,m.RowUno	
--,m.MapCode	
,m.ApplicationType	
,m.StageTable	
,m.SourceColumnLabel	
,m.TargetColumn	
,m.DataType
FROM OPENJSON(@json) WITH (   
 MapCode     varchar(15) '$.MapCode'
,MapDesc     varchar(15) '$.MapDesc'
,FileMask	 varchar(15) '$.FileMask'
,FileType	 varchar(15) '$.FileType'
,Inactive	 varchar(15) '$.Inactive'
,ReceiptID	 varchar(15) '$.ReceiptID'
,TargetTable varchar(15) '$.TargetTable'
,XLSheetName varchar(15) '$.XLSheetName'
,Map		 nvarchar(max) '$._AAC_CRMAP' as JSON

) AS z
CROSS APPLY OPENJSON(z.Map) WITH (
 MapCode             varchar(max) '$.MapCode'
,RowUno				 varchar(max) '$.RowUno'
,MapCode			 varchar(max) '$.MapCode'
,ApplicationType	 varchar(max) '$.ApplicationType'
,StageTable			 varchar(max) '$.StageTable'
,SourceColumnLabel 	 varchar(max) '$.SourceColumnLabel'
,TargetColumn		 varchar(max) '$.TargetColumn'
,DataType			 varchar(max) '$.DataType'



) m

)
insert into _AAC_CRMAP (MapCode	,ApplicationType	,StageTable	,SourceColumnLabel	,TargetColumn	,DataType)
select MapCode	,ApplicationType	,StageTable	,SourceColumnLabel	,TargetColumn	,DataType from map


select * from _aac_crmap where mapcode = @Mapcode
--set identity_insert _AAC_CRMAPZ  off
--set identity_insert _AAC_CRMAP  off
Commit
GO

/****** Object:  StoredProcedure [dbo].[_spaac_ImportCR]    Script Date: 1/22/2023 8:49:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




/****** Object:  StoredProcedure [EXTERNAL\snielsen].[_spaac_ImportCR]    Script Date: 9/7/2022 10:27:59 PM ******/

CREATE proc [dbo].[_spaac_ImportCR] @BatchID integer, @NoCommit as bit = 1, @TranSeqFilter integer = -1
as
--11/21/2022 Add logic for type "Z" (multi matter)
declare @SessionOUT integer
declare @ReceiptNumOUT integer
declare @ThisIsMyTransaction bit = 0

if @@trancount = 0
BEGIN
	Set @ThisIsMyTransaction = 1
	BEGIN transaction
END
select 'Create Session'
exec [_spaac_Cr_Create_Session] @BatchID =  @BatchID, @transeqfilter = @TranSeqFilter, @Session = @SessionOUT OUTPUT
select 'Session Created:' + convert(varchar(12),@SessionOUT)
-------------------------------
select 'Begin Receipts Loop'
declare Receipts insensitive cursor  for
  select distinct transeq from _aac_cr_load_b where applicationtype <> 'R' and batchid = @BatchID 
  and (@TranSeqFilter = transeq or @TranSeqFilter = -1)
  order by transeq
declare @TranSeq integer
declare @tranlineseq integer
declare @AmountApplied money = -1
open  Receipts
while 1=1
BEGIN -- Process Cursor Row
	fetch next from Receipts into @TranSeq	
	if @@fetch_status = -1 break -- EXIT LOOP
	--if @transeq = 18 -- FOr testing
	--begin
		Select 'Create ReceiptNum for transeq=' + convert(varchar(12),@TranSeq)
		exec [_spaac_Cr_Create_Receip] @BatchID, @SessionOut, @TranSeq, @ReceiptNumOUT OUTPUT
		Select 'ReceiptNum  Created' + convert(varchar(12),@ReceiptNumOUT)

		-- Process I transactions within transeq  ------------------------I I I I I I I I I I I I I I I I I I I I I I I I  Loop Start
		if exists (select 1 from  _aac_cr_load_b where applicationtype = 'I' and transeq = @TranSeq and batchid = @batchid)
		BEGIN
		    select convert(varchar(10),count(*)) + ' I type trans exist' from  _aac_cr_load_b where applicationtype = 'I' and transeq = @TranSeq and batchid = @batchid
			declare BillApps insensitive cursor  for
				select tranlineseq from _aac_cr_load_b  
				where applicationtype = 'I' and transeq = @TranSeq and batchid = @batchid
				order by TranLineSeq 

			open  BillApps
			while 1=1
			BEGIN -- Process Invoice Applicaitons
				fetch next from BillApps into @tranlineseq
				if @@fetch_status = -1 break -- EXIT LOOP
				select 'I-Application', @BatchID 'BatchID', @TranSeq 'TranSeq', @SessionOUT 'Session', @ReceiptnumOUT 'ReceiptNum', @tranlineseq 'tranlineseq'
				exec _spaac_Cr_Bill_Application 
				 @ReceiptNum	= @ReceiptNumOUT 
				,@BatchID		= @BatchID		
				,@TranSeq		= @TranSeq
				,@TranLineSeq	= @TranLineSeq
				,@AmtApplied	= @AmountApplied OUTPUT
				select '_spaac_Cr_Bill_Application applied ' + isnull(convert(varchar(12),@AmountApplied),'{NULL}')

			END
			close BillApps
			deallocate BillApps
		END
		-- Process I transactions within transeq  ------------------------I I I I I I I I I I I I I I I I I I I I I I I I  Loop END
		
		
		-- Process Z transactions within transeq  ------------------------Z Z Z Z Z Z Z Z Z Z Z Z Z Z Z Z Z Z Z Z Z Z Z Z  Loop Start
		--Identify all matters for bill and call _spaac_Cr_Bill_Application for each matter
		if exists (select 1 from  _aac_cr_load_b where applicationtype = 'Z' and transeq = @TranSeq and batchid = @batchid)
		BEGIN
		    declare @UnallocatedBal numeric(17,4), @BMARTotal numeric(17,4), @InvoiceNo integer
		    select convert(varchar(10),count(*)) + ' Z type trans exist' from  _aac_cr_load_b where applicationtype = 'Z' and transeq = @TranSeq and batchid = @batchid
			declare BillApps insensitive cursor  for
				select tranlineseq , amt, InvoiceNo  from _aac_cr_load_b  
				where applicationtype = 'Z' and transeq = @TranSeq and batchid = @batchid
				order by TranLineSeq 

			open  BillApps
			while 1=1
			BEGIN -- Process Invoice Applicaitons --  ONE PER MATTER, Track balance and stop at zero
				fetch next from BillApps into @tranlineseq, @unallocatedbal, @InvoiceNo
				if @@fetch_status = -1 break -- EXIT LOOP
				   select '[_spaac_ImportCR] ZApp - InvoiceNo=' + isnull(convert(varchar(15),@InvoiceNo),'NULL')
				   declare BM insensitive cursor for 
				        select matter_uno, bill_tran_uno, TOTAL_AR  from blt_billm bm join blt_bill bl on bm.bill_tran_uno = bl.tran_uno where bill_num = @InvoiceNo and bl.LAST_BILL_ACTION <> 'REVR' and bm.ar_status = 'O'
					Declare @MatterUno integer, @BTU Integer
				--  cursor for all matters on outstanding matters for this bill
				--    billm -> bl for @InvoiceNo and arstatus = 'O'
				--  loop on matter number while not end and @unallocatedbal > 0
				-- Call exec _spaac_Cr_Bill_Application as below but add @MatterUno parameter
						open BM
						while 1=1
						BEGIN -- Process Invoice Applicaitons --  ONE PER MATTER, Track balance and stop at zero
							fetch next from BM into @MatterUno, @BTU, @BMArTotal
							if @@fetch_status = -1 break -- EXIT LOOP
			
							select 'Z-Application', @BatchID 'BatchID', @TranSeq 'TranSeq', @SessionOUT 'Session', @ReceiptnumOUT 'ReceiptNum', @tranlineseq 'tranlineseq', @BMARTotal 'AmountToAllocate'

							exec _spaac_Cr_Bill_Application 
							 @ReceiptNum	= @ReceiptNumOUT 
							,@BatchID		= @BatchID		
							,@TranSeq		= @TranSeq
							,@TranLineSeq	= @TranLineSeq
							,@AmtApplied	= @AmountApplied OUTPUT
							,@MatterUno		= @MatterUno
							,@AmountToAllocate = @BMARTotal
							select '_spaac_Cr_Bill_Application applied ' + convert(varchar(12),@AmountApplied)
							set @unallocatedbal = @unallocatedbal - @AmountApplied
						end
						close BM
						deallocate BM
			END
			close BillApps
			deallocate BillApps
		END
		-- Process Z transactions within transeq  ------------------------Z Z Z Z Z Z Z Z Z Z Z Z Z Z Z Z Z Z Z Z Z Z Z Z  Loop END


		-- Process C transactions within transeq  ------------------------C C C C C C C C C C C C C C C C C C C C C C C C  Loop Start
		if exists (select 1 from  _aac_cr_load_b where applicationtype = 'C' and transeq = @TranSeq and batchid = @batchid)
		BEGIN
		 select convert(varchar(10),count(*)) + ' C type trans exist' from  _aac_cr_load_b where applicationtype = 'C' and transeq = @TranSeq and batchid = @batchid
			declare BillApps insensitive cursor  for
				select tranlineseq from _aac_cr_load_b  
				where applicationtype = 'C' and transeq = @TranSeq and batchid = @batchid
				order by TranLineSeq 
			
			open  BillApps
			while 1=1
			BEGIN -- Process Invoice Applicaitons
				fetch next from BillApps into @tranlineseq
				if @@fetch_status = -1 break -- EXIT LOOP
				select 'C-Application', @BatchID 'BatchID', @TranSeq 'TranSeq', @SessionOUT 'Session', @ReceiptnumOUT 'ReceiptNum', @tranlineseq 'tranlineseq'
				exec _spaac_CL_Credit_Application 
				 @ReceiptNum	= @ReceiptNumOUT 
				,@BatchID		= @BatchID		
				,@TranSeq		= @TranSeq
				,@TranLineSeq	= @TranLineSeq
				,@AmtApplied	= @AmountApplied OUTPUT
				select '_spaac_CL_Credit_Application applied ' + convert(varchar(12),@AmountApplied)

			END
			close BillApps
			deallocate BillApps
		END
		-- Process C transactions within transeq  ------------------------C C C C C C C C C C C C C C C C C C C C C C C C  Loop END

			-- Process M transactions within transeq  ------------------------M M M M M M M M M M M M M M M M M M M M M M M M   Loop Start
		if exists (select 1 from  _aac_cr_load_b where applicationtype = 'M' and transeq = @TranSeq and batchid = @batchid)
		BEGIN
		 select convert(varchar(10),count(*)) + ' M type trans exist' from  _aac_cr_load_b where applicationtype = 'M' and transeq = @TranSeq and batchid = @batchid
			declare BillApps insensitive cursor  for
				select tranlineseq from _aac_cr_load_b  
				where applicationtype = 'M' and transeq = @TranSeq and batchid = @batchid
				order by TranLineSeq 
			
			open  BillApps
			while 1=1
			BEGIN -- Process Invoice Applicaitons
				fetch next from BillApps into @tranlineseq
				if @@fetch_status = -1 break -- EXIT LOOP
				select 'M-Application', @BatchID 'BatchID', @TranSeq 'TranSeq', @SessionOUT 'Session', @ReceiptnumOUT 'ReceiptNum', @tranlineseq 'tranlineseq'
				exec _spaac_mt_Credit_Application 
				 @ReceiptNum	= @ReceiptNumOUT 
				,@BatchID		= @BatchID		
				,@TranSeq		= @TranSeq
				,@TranLineSeq	= @TranLineSeq
				,@AmtApplied	= @AmountApplied OUTPUT
				select '_spaac_MT_Credit_Application applied ' + convert(varchar(12),@AmountApplied)

			END
			close BillApps
			deallocate BillApps
		END
			-- Process M transactions within transeq  ------------------------M M M M M M M M M M M M M M M M M M M M M M M M   Loop eND


	--end
END

Update _aac_cr_load_b set _importDate = getdate(), _ImportStatus = 'C', _SESSION = @SessionOUT
where BatchID = @BatchID 

close Receipts
deallocate Receipts
select 'EndReceiptsLoop'
if @ThisIsMyTransaction = 1
BEGIN
	if @NoCommit = 1
		Rollback	
	else
		Commit
END
GO

/****** Object:  StoredProcedure [dbo].[SpAacCRPreValidateData_B]    Script Date: 1/22/2023 8:49:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO







CREATE procedure [dbo].[SpAacCRPreValidateData_B] @batchID int, @debug bit = 0
--with encryption
as
	set nocount on
	declare @trackingNumber int

	--SET DATEFORMAT dmy;
declare @RowCount integer
declare @Step varchar(60)
BEGIN TRY

	EXEC SpAacCRLogProcedure @ObjectID = @@PROCID, @BatchID = @batchID, @ProcedurePhase = 'BEG'

	if exists (select 1 from _aac_cr_load_b where BatchID = @Batchid and isnull(_session,0) <> 0)
	BEGIN
		insert into _AacCRImpErrorLog (Severity, BatchID, Phase, ErrorCode, TrackingNumber, IdentifierCode, ErrorValue, ErrorDesc )
				select Severity			= 2
					, BatchID			= @batchID
					, Phase				= 'VALIDATE'
					, ErrorCode			= 'Batch already processed'
					, TrackingNumber	= 0
					, IdentifierCode	= NULL
					, ErrorValue		=  'Cash receipt session ' + convert(char(16),_SESSION) + ' has already been created for this batch'
					, ErrorDesc			= 'Cannot re-process a batch to create duplicate session.'
				from  _aac_cr_load_b cr
				where cr.BatchID = @BatchID and isnull(_session,0) <> 0
		--RETURN -1
	END


	IF @DEBUG = 1 PRINT 'BatchID: ' + convert(varchar, @batchID)

	--PREPROCESSING--
	/*
	Apply processing rules to clean data:
	 - Blank AMT columns should be treated as zeroes
	 - Compute LineSeq for batch as transeq * 1000 + TranLineSeq
	 - if no C.M specified:
		If single matter bill (invoiceno) then get matter from BILLM table
		If multi-matter, single payor and no C.M specified - change to client credit (UAC)
	 - If header(R) total does not foot to detail totals, update header total

	 Todo?  
	 - if multi-matter, single payor and no C.M specified - change to client credit (UAC)

	*/


----------------------------------------------------------------------------------
--Preserve original staged data - this should execute on the first validation pass
declare @Columns varchar(max) = ''
declare @select  varchar(max) = ''
DECLARE @CMD NVARCHAR(MAX)
if not exists (select 1 from _aac_cr_load_b where batchid = (0 - @batchid))
BEGIN
	select @Columns = @columns + ',' + a.name from sys.columns a join sys.objects o on a.object_id = o.object_id and o.name = '_aac_cr_load_b'
	set @Columns = substring(@columns,2,99999)
	set @select = replace (@columns,'batchid','0 - batchid')
	SET @CMD = 'insert into _aac_cr_load_b(' + @columns + ') select ' + @select + ' from _aac_cr_load_b where batchid = ' + convert(varchar(10),@batchid)
	EXEC SP_EXECUTESQL @CMD
	IF @DEBUG = 1 PRINT 'COPIED BATCH TO BACKUP (NEGATIVE BATCHID)'
END
ELSE --For subsequent validations, restore original uploaded data
BEGIN
--	delete from _aac_cr_load_b where batchid = @batchid
	select @Columns = @columns + ',' + a.name from sys.columns a join sys.objects o on a.object_id = o.object_id and o.name = '_aac_cr_load_b'
	set @Columns = substring(@columns,2,99999)
	set @select = replace (@columns,'batchid','0 - batchid')
	sET @cmd = 'delete from _aac_cr_load_b where batchid = ' + CONVERT(VARCHAR(10),@batchid) + '; '
	SET @CMD = @cmd + 'insert into _aac_cr_load_b(' + @columns + ') select ' + @select + ' from _aac_cr_load_b where batchid = ' + convert(varchar(10),(0 - @batchid))
	EXEC SP_EXECUTESQL @CMD
	IF @DEBUG = 1 PRINT 'RESTORED ORIGINAL BATCH FROM BACKUP (NEGATIVE BATCHID)'
END
----------------------------------------------------------------------------------

	IF @DEBUG = 1 PRINT 'Cleaning Data'
update _aac_cr_load_b set InvoiceNo = '' where isnumeric(invoiceNo) <> 1 and BatchID = @BatchID
update _aac_cr_load_b set LineSeq = TranSeq * 10000 + TranLineSeq  where BatchID = @BatchID
update _aac_cr_load_b set amt = dbo.FN_RemoveNonNumeric (amt) where BatchID = @BatchID 
update _aac_cr_load_b set amt = 0.00 where BatchID = @BatchID and isnumeric(amt) = 0
update _aac_cr_load_b set payor = client where isnull(client,'') <> '' and isnull(payor,'') = ''
update _aac_cr_load_b set receiptdate = 
	case when isnumeric(receiptdate) = 1 and convert(integer,substring(receiptdate,1,2)) > 12 then 
			convert(date,'20' + substring(receiptdate,1,2) + '-' + substring(receiptdate,3,2) + '-' + substring(receiptdate,5,2))
		 when isnumeric(receiptdate) = 1 and convert(integer,substring(receiptdate,5,2)) > 12 then 
			convert(date,'20' + substring(receiptdate,5,2) + '-' + substring(receiptdate,1,2) + '-' + substring(receiptdate,3,2))
  else '' end
    where isnumeric(receiptdate) = 1 and (ISDATE (receiptdate) <> 1 or abs(year(receiptdate) - year(getdate())) > 1)



set @Step = 'Derive C.M FOR SINGLE MATER BILLS' IF @DEBUG = 1 PRINT 'Step: ' + @step
-- Look up bill number if specified.  If single matter bill, and client or matter not specified, use matter and client from that.
		insert into _AacCRImpErrorLog (Severity, BatchID, Phase, ErrorCode, TrackingNumber, IdentifierCode, ErrorValue, ErrorDesc )
				select Severity			= 0
					, BatchID			= @batchID
					, Phase				= 'VALIDATE'
					, ErrorCode			= 'Derived Client Matter'
					, TrackingNumber	= cr.LineSeq
					, IdentifierCode	= cr.ReceiptDate
					, ErrorValue		=  'Matter ' + m.client_code + '.' + m.matter_code + ' derived from single-matter Invoiceno' + invoiceno
					, ErrorDesc			= @Step
				from  _aac_cr_load_b cr
				join blt_bill bl on cr.InvoiceNo = bl.bill_num
				join blt_billm bm on bl.tran_uno = bm.bill_tran_uno and (select count(*) from blt_billm where blt_billm.bill_tran_uno = bl.tran_uno) = 1
				join hbm_matter m on bm.matter_uno = m.matter_uno
				where cr.BatchID = @BatchID and applicationtype = 'I'
				and isnull(invoiceno,'') <> ''
				--and ((isnull(cr.client ,'')<> m.client_code or isnull(cr.matterid,'') <> m.matter_code))  --< this will force the C.M to match the bill
				and (isnull(cr.client ,'') = '' or isnull(cr.matterid,'') = '')

											
		update cr set payor = m.client_code, client = m.client_code,matterid = m.matter_code
			from  _aac_cr_load_b cr
				join blt_bill bl on cr.InvoiceNo = bl.bill_num
				join blt_billm bm on bl.tran_uno = bm.bill_tran_uno and (select count(*) from blt_billm where blt_billm.bill_tran_uno = bl.tran_uno) = 1
				join hbm_matter m on bm.matter_uno = m.matter_uno
				where cr.BatchID = @BatchID and applicationtype = 'I'
				and isnull(invoiceno,'') <> ''
				--and ((isnull(cr.client ,'')<> m.client_code or isnull(cr.matterid,'') <> m.matter_code))  --< this will force the C.M to match the bill
				and (isnull(cr.client ,'') = '' or isnull(cr.matterid,'') = '')
				Set @Rowcount = @@Rowcount
				IF @DEBUG = 1 PRINT @step + ' Rows Updated:' + convert(varchar(10),@rowcount) 

																																						  

set @Step = 'Missing C.M FOR Multi MATER BILLS' IF @DEBUG = 1 PRINT 'Step: ' + @step
--If Multimatter bill, and payor not specified, but invoice is -- record as UAC to specified credit matter  Use BILLP to identify Client------------------------
		;With BillmCount as (select bill_tran_uno, count(*) 'count' from blt_billm group by bill_tran_uno)
		insert into _AacCRImpErrorLog (Severity, BatchID, Phase, ErrorCode, TrackingNumber, IdentifierCode, ErrorValue, ErrorDesc )
				select Severity			= 0
					, BatchID			= @batchID
					, Phase				= 'VALIDATE'
					, ErrorCode			= 'Derived Payor - Multi-matter bill'
					, TrackingNumber	= cr.LineSeq
					, IdentifierCode	= cr.ReceiptDate
					, ErrorValue		=  'Payor ' + c.client_code + ' derived from Multi-matter Invoiceno' + convert(varchar(16),invoiceno)
					, ErrorDesc			= @Step
					from  _aac_cr_load_b cr
			join blt_bill bl on cr.InvoiceNo = bl.bill_num
			join blt_billp bp on bl.tran_uno = bp.bill_tran_uno
			join hbm_client c on bp.PAYR_CLIENT_UNO = c.client_uno
			join BillmCount bm on bl.tran_uno = bm.bill_tran_uno and bm.count > 1
			where cr.BatchID = @BatchID and applicationtype = 'I'
			and isnull(client,'') = '' 
			and isnull(invoiceno,'') <> ''
		
		;With BillmCount as (select bill_tran_uno, count(*) 'count' from blt_billm where total_ar <> 0.0 group by bill_tran_uno)
		--select cr.amt,  c.client_code, *
		--update cr set payor = c.client_code, client = c.client_code, MatterID = '0001001', ApplicationType = 'M', Credittype = 'C',
		--	DistDescription = 'CR Upload to MultiMatter bill ' + cr.invoiceNo + ' but cannot derive matter.  Apply to General Matter'
		    update cr set   payor = c.client_code, client = c.client_code
			from  _aac_cr_load_b cr
			join blt_bill bl on cr.InvoiceNo = bl.bill_num
			join blt_billp bp on bl.tran_uno = bp.bill_tran_uno
			join hbm_client c on bp.PAYR_CLIENT_UNO = c.client_uno
			join BillmCount bm on bl.tran_uno = bm.bill_tran_uno and bm.count > 1
			where cr.BatchID = @BatchID and applicationtype = 'I'
			and isnull(client,'') = '' 
			and isnull(invoiceno,'') <> ''
			Set @Rowcount = @@Rowcount
			IF @DEBUG = 1 PRINT @step + ' Rows Updated:' + convert(varchar(10),@rowcount) 

																																					  
									 

IF @DEBUG = 1 PRINT 'Derive C.M FOR Multi PAYOR BILLS ' --------------------------------------------------------------------------------------
--If Multipayor bill, and payor not specified -- record as UAC to specified credit matter--client from Billm Matter----------------------------
		;With BillpCount as (select bill_tran_uno, count(*) 'count' from blt_billp where total_ar <> 0.0 group by bill_tran_uno)
		insert into _AacCRImpErrorLog (Severity, BatchID, Phase, ErrorCode, TrackingNumber, IdentifierCode, ErrorValue, ErrorDesc )
				select Severity			= 0
					, BatchID			= @batchID
					, Phase				= 'VALIDATE'
					, ErrorCode			= 'Convert to General Matter Credit'
					, TrackingNumber	= cr.LineSeq
					, IdentifierCode	= cr.ReceiptDate
					, ErrorValue		=  'Client ' + c.client_code + ' General matter to receive Unapplied Cash Credit for Invoiceno:' + invoiceno
					, ErrorDesc			= 'Multi-matter bill and no matter specified, applying to designated matter (UAC).'
			from  _aac_cr_load_b cr
			join blt_bill bl on cr.InvoiceNo = bl.bill_num
			join blt_billm bm on bl.tran_uno = bm.bill_tran_uno
			join hbm_matter m on bm.matter_uno = m.matter_uno
			join hbm_client c on m.CLIENT_UNO = c.client_uno
			join BillpCount bp on bl.tran_uno = bp.bill_tran_uno and bp.count > 1
			where cr.BatchID = @BatchID and applicationtype = 'I'
			and isnull(client,'') = '' 
			and isnull(invoiceno,'') <> ''
		
		;With BillpCount as (select bill_tran_uno, count(*) 'count' from blt_billp where total_ar <> 0.0 group by bill_tran_uno)
		--select cr.amt,  c.client_code, *
		update cr set payor = c.client_code, client = c.client_code, MatterID = '0001001', ApplicationType = 'M', Credittype = 'C',
			DistDescription = 'CR Upload to MultiMatter bill ' + cr.invoiceNo + ' but cannot derive matter.  Apply to General Matter'
			from  _aac_cr_load_b cr
			join blt_bill bl on cr.InvoiceNo = bl.bill_num
			join blt_billm bm on bl.tran_uno = bm.bill_tran_uno
			join hbm_matter m on bm.matter_uno = m.matter_uno
			join hbm_client c on m.CLIENT_UNO = c.client_uno
			join BillpCount bp on bl.tran_uno = bp.bill_tran_uno and bp.count > 1
			where cr.BatchID = @BatchID and applicationtype = 'I'
			and isnull(client,'') = '' 
			and isnull(invoiceno,'') <> ''
IF @DEBUG = 1 PRINT 'Updated Multi-Payor to Credit.  Rows Updated:' + convert(varchar(10),@@rowcount) 

--At this point - MultiMatter and MultiPayor bills are designated as UAC payments to credit amtter
-- TODO? If Multi-matter bill and amount matches AR amount, split into multiple bill applications by matter



set @Step = 'Validate Header Totals against Details' IF @DEBUG = 1 PRINT 'Step: ' + @step
-- Header total should match totals for all details within receipt  TRANSEQ is used to batch receitps
;with BatchTot as (select transeq, sum(convert(numeric(17,4),amt)) 'DetailTotal' from _aac_cr_load_b  
				   where batchid = @BatchID and applicationtype <> 'R' group by transeq)
insert into _AacCRImpErrorLog (Severity, BatchID, Phase, ErrorCode, TrackingNumber, IdentifierCode, ErrorValue, ErrorDesc )
		select Severity			= 0
			, BatchID			= @batchID
			, Phase				= 'VALIDATE'
			, ErrorCode			= 'Header Detail Balance'
			, TrackingNumber	= cr.LineSeq
			, IdentifierCode	= cr.ReceiptDate
			, ErrorValue		=  'Receipt header Amt, ' + convert(varchar(20),cr.amt ) + ', <> detail total, ' + convert (varchar(40), d.detailtotal ) + ': Adjusted to match'
			, ErrorDesc			= @Step
		 from _aac_cr_load_b cr
join batchtot d on cr.transeq = d.transeq
where cr.batchid = @BatchID and cr.applicationtype = 'R' and  convert(numeric(17,4),cr.amt) <> d.detailtotal


;with BatchTot as (select transeq, sum(convert(numeric(17,4),amt)) 'DetailTotal'
					from _aac_cr_load_b where batchid = @BatchID and applicationtype <> 'R' group by transeq)
update cr set amt = d.DetailTotal
from _aac_cr_load_b cr
join batchtot d on cr.transeq = d.transeq
where cr.batchid = @BatchID and cr.applicationtype = 'R' and  convert(numeric(17,4),cr.amt) <> d.detailtotal
Set @Rowcount = @@Rowcount
IF @DEBUG = 1 PRINT @step + ' Rows Updated:' + convert(varchar(10),@rowcount) 
																															   

-------------------------------------------------------------------------------------------------------------------------------
set @Step = 'Missing Header Payor' IF @DEBUG = 1 PRINT 'Step: ' + @step
--  Header row must indicate payor - if not specified, get form first detail row (Details should all have a client at this point)
;with payor as(
select payor, transeq, Seq = row_number() over (partition by transeq order by tranlineseq)
from _aac_cr_load_b 
where isnull(payor,'') <> ''  and ApplicationType <>'R'
)
insert into _AacCRImpErrorLog (Severity, BatchID, Phase, ErrorCode, TrackingNumber, IdentifierCode, ErrorValue, ErrorDesc )
		select Severity			= 0
			, BatchID			= @batchID
			, Phase				= 'VALIDATE'
			, ErrorCode			= 'Set Header Payor'
			, TrackingNumber	= cr.LineSeq
			, IdentifierCode	= cr.ReceiptDate
			, ErrorValue		=  'Missing payor in header set to initial client from detail ' + convert(varchar(20),p.payor ) 
			, ErrorDesc			= @Step
from _aac_cr_load_b cr
join payor p on cr.transeq = p.transeq and seq = 1
where cr.batchid = @BatchID and  cr.ApplicationType = 'R' and  isnull(cr.payor,'') = ''


;with payor as(
select payor, transeq, Seq = row_number() over (partition by transeq order by tranlineseq)
from _aac_cr_load_b
where isnull(payor,'') <> ''  and ApplicationType <>'R'
)
Update cr set payor = p.payor
from _aac_cr_load_b cr
join payor p on cr.transeq = p.transeq and seq = 1
where cr.batchid = @BatchID and cr.ApplicationType = 'R' and isnull(cr.payor,'')  = ''
Set @Rowcount = @@Rowcount
IF @DEBUG = 1 PRINT @step + ' Rows Updated:' + convert(varchar(10),@rowcount) 

------------------------------------------------------------------------------------------------------------
set @Step = 'Bill is Settled' IF @DEBUG = 1 PRINT 'Step: ' + @step

									 
--Rule - If indicated bill is settled - change to matter level UAC credit
	insert into _AacCRImpErrorLog (Severity, BatchID, Phase, ErrorCode, TrackingNumber, IdentifierCode, ErrorValue, ErrorDesc )
			select Severity			= 0
				, BatchID			= @batchID
				, Phase				= 'VALIDATE'
				, ErrorCode			= 'CR Bill Settled'
				, TrackingNumber	= cr.LineSeq
				, IdentifierCode	= cr.ReceiptDate
				, ErrorValue		= 'CR Bill is settled'
				, ErrorDesc			= @Step

		from _aac_cr_load_b cr
		join hbm_matter m on cr.client = m.client_code and cr.MatterID = m.matter_code
		join blt_bill bl on cr.invoiceNo = bl.bill_num
		join blt_billm bm on bl.tran_uno = bm.bill_tran_uno and bm.matter_uno = m.matter_uno and bm.AR_status = 'S'
		where applicationtype = 'I' and cr.batchid = @BatchID and isnull( matterid ,'') <> ''
		
  
	 update cr set  ApplicationType = 'M', Credittype = 'C', DistDescription = 'CR Upload to settled bill ' + cr.invoiceNo 
		from _aac_cr_load_b cr
		join hbm_matter m on cr.client = m.client_code and cr.MatterID = m.matter_code
		join blt_bill bl on cr.invoiceNo = bl.bill_num
		join blt_billm bm on bl.tran_uno = bm.bill_tran_uno and bm.matter_uno = m.matter_uno and bm.AR_status = 'S'
		where applicationtype = 'I' and cr.batchid = @BatchID and isnull( matterid ,'') <> ''
		Set @Rowcount = @@Rowcount
		IF @DEBUG = 1 PRINT @step + ' Rows Updated:' + convert(varchar(10),@rowcount) 
																															  

	-----------------------------------------------------------------------------------------------------------------------------
set @Step = 'Cash Receipt <= AR (info only)' IF @DEBUG = 1 PRINT 'Step: ' + @step
--Rule - If indicated bill is settled - change to matter level UAC credit
	insert into _AacCRImpErrorLog (Severity, BatchID, Phase, ErrorCode, TrackingNumber, IdentifierCode, ErrorValue, ErrorDesc )
			select Severity			= 0
				, BatchID			= @batchID
				, Phase				= 'VALIDATE'
				, ErrorCode			= 'CR Amount <= AR balance'
				, TrackingNumber	= cr.LineSeq
				, IdentifierCode	= cr.ReceiptDate
				, ErrorValue		= 'CR Partial Payment'
				, ErrorDesc			= @Step

		from _aac_cr_load_b cr
		join hbm_matter m on cr.client = m.client_code and cr.MatterID = m.matter_code
		join blt_bill bl on cr.invoiceNo = bl.bill_num
		join blt_billm bm on bl.tran_uno = bm.bill_tran_uno and bm.matter_uno = m.matter_uno and bm.AR_status = 'O'
		where applicationtype = 'I' and cr.batchid = @BatchID and isnull( matterid ,'') <> ''
		and cr.Amt <= bm.TOTAL_AR
		
  
	 --update cr set  ApplicationType = 'M', Credittype = 'C', DistDescription = 'CR Upload to settled bill ' + cr.invoiceNo 
		--from _aac_cr_load_b cr
		--join hbm_matter m on cr.client = m.client_code and cr.MatterID = m.matter_code
		--join blt_bill bl on cr.invoiceNo = bl.bill_num
		--join blt_billm bm on bl.tran_uno = bm.bill_tran_uno and bm.matter_uno = m.matter_uno and bm.AR_status = 'O'
		--where applicationtype = 'I' and cr.batchid = @BatchID and isnull( matterid ,'') <> ''
	 --IF @DEBUG = 1 PRINT 'Settled bill - set to matter credit.  Rows Updated:' + convert(varchar(10),@@rowcount) 
	-----------------------------------------------------------------------------------------------------------------------------
																							  
																		 
																															
						
						  
					 
											
								 
									 
									 
																	 

	--++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ For multi-matter bill payments, if overpaid, create matter credit
																				
												
																											 
																					   
						  

															
																															  
																	
																				  
																								
																					  
																				 
					  
																				
												
																											 
																					   
						  
																												  


----------	set @Step = 'Multi-matter Overpayment (CR > AR)' IF @DEBUG = 1 PRINT 'Step: ' + @step
----------	insert into _AacCRImpErrorLog (Severity, BatchID, Phase, ErrorCode, TrackingNumber, IdentifierCode, ErrorValue, ErrorDesc )
----------			select Severity			= 0
----------				, BatchID			= @batchID
----------				, Phase				= 'VALIDATE'
----------				, ErrorCode			= 'CR MultiMatterAmount > AR balance'
----------				, TrackingNumber	= cr.LineSeq
----------				, IdentifierCode	= cr.ReceiptDate
----------				, ErrorValue		= 'CR Over Payment on MM Bill'
----------				, ErrorDesc			= @Step

----------		--select InvoiceNo ,	transeq, cr.Amt , sum(bm.total_ar) 'TotalAR' 
----------		from  _aac_cr_load_b cr
----------		where cr.batchid = @batchID and cr.applicationtype = 'R' and cr.transeq in (select transeq
----------								from _aac_cr_load_b cr
----------								join blt_bill bl on cr.InvoiceNo = bl.BILL_NUM and LAST_BILL_ACTION <> 'REVR'
----------								join blt_billm bm on bl.tran_uno = bm.bill_tran_uno
----------								where cr.batchid = @batchID and cr.ApplicationType in ('z')
----------								group by invoiceno, transeq, cr.amt
----------								having cr.Amt > sum(bm.total_ar)
----------							)
------------ Create matter credit for overpayment																												!!!!!!!not picking up matter  TODO TODO

----------	insert into _aac_cr_load_b (ApplicationType,ReceiptDate,BankCode,ReceiptType,CheckNum,DrawnBy,Payor,Client,MatterID,InvoiceNo,
----------		                          Amt,BatchID,LineSeq,TranSeq,TranLineSeq,
----------								  _SourceFile,_ImportStatus,_StagingUser,_StagingDate,_ImportDate,TranRef)
----------		select 'M',cr.ReceiptDate,cr.BankCode,cr.ReceiptType,cr.CheckNum,cr.DrawnBy,crH.Payor,crh.Payor,'0001001',cr.InvoiceNo,
----------		                          cr.amt - (
----------								  select sum(total_ar) from blt_billm bm1 join blt_bill bl1 on bm1.bill_tran_uno = bl1.tran_uno and bl1.bill_num = cr.invoiceno and last_bill_action <> 'REVR'
----------								  ) 'amt',
----------								  cr.BatchID,cr.LineSeq,cr.TranSeq,cr.TranLineSeq,
----------								  cr._SourceFile,cr._ImportStatus,cr._StagingUser,cr._StagingDate,cr._ImportDate,cr.TranRef
----------		from _aac_cr_load_b cr
----------		left join  _aac_cr_load_b crH on cr.transeq = crh.transeq and crh.ApplicationType = 'R' and crh.batchid = cr.batchid
----------			where cr.batchid = @batchID 
----------			and cr.applicationtype = 'z' and cr.transeq in (select transeq
----------								from _aac_cr_load_b cr
----------								join blt_bill bl on cr.InvoiceNo = bl.BILL_NUM and LAST_BILL_ACTION <> 'REVR'
----------								join blt_billm bm on bl.tran_uno = bm.bill_tran_uno
----------								where cr.batchid = @batchID and cr.ApplicationType in ('z')
----------								group by invoiceno, transeq, cr.amt
----------								having cr.Amt > sum(bm.total_ar)
----------							)
----------		Set @Rowcount = @@Rowcount
----------		IF @DEBUG = 1 PRINT @step + ' M Rows Inserted:' + convert(varchar(10),@rowcount) 

 -------- --Update Z distribution to deduct overpaymetn amount
 -------- update cr set amt =  (select sum(total_ar) from blt_billm bm1 join blt_bill bl1 on bm1.bill_tran_uno = bl1.tran_uno and bl1.bill_num = cr.invoiceno and last_bill_action <> 'REVR')
	--------from _aac_cr_load_b cr
	--------	where cr.batchid = @batchID and cr.applicationtype = 'z' and cr.transeq in (select transeq
	--------							from _aac_cr_load_b cr
	--------							join blt_bill bl on cr.InvoiceNo = bl.BILL_NUM and LAST_BILL_ACTION <> 'REVR'
	--------							join blt_billm bm on bl.tran_uno = bm.bill_tran_uno
	--------							where cr.batchid = @batchID and cr.ApplicationType in ('z')
	--------							group by invoiceno, transeq, cr.amt
	--------							having cr.Amt > sum(bm.total_ar)
	--------						)
 -------- 	Set @Rowcount = @@Rowcount
	--------IF @DEBUG = 1 PRINT @step + ' Rows Updated:' + convert(varchar(10),@rowcount) 

	--++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

--------set @Step = 'Single matter CR > AR' IF @DEBUG = 1 PRINT 'Step: ' + @step
----------Rule - If indicated bill is settled - change to matter level UAC credit
--------	insert into _AacCRImpErrorLog (Severity, BatchID, Phase, ErrorCode, TrackingNumber, IdentifierCode, ErrorValue, ErrorDesc )
--------			select Severity			= 0
--------				, BatchID			= @batchID
--------				, Phase				= 'VALIDATE'
--------				, ErrorCode			= 'CR Amount > AR balance'
--------				, TrackingNumber	= cr.LineSeq
--------				, IdentifierCode	= cr.ReceiptDate
--------				, ErrorValue		= 'CR Over Payment'
--------				, ErrorDesc			= @Step

--------		from _aac_cr_load_b cr
--------		join hbm_matter m on cr.client = m.client_code and cr.MatterID = m.matter_code
--------		join blt_bill bl on cr.invoiceNo = bl.bill_num
--------		join blt_billm bm on bl.tran_uno = bm.bill_tran_uno and bm.matter_uno = m.matter_uno and bm.AR_status = 'O'
--------		where applicationtype = 'I' and cr.batchid = @BatchID and isnull( matterid ,'') <> ''
--------		and cr.Amt > bm.TOTAL_AR

--------		-- Insert 'M' rows to credit matter for overpayment amount
--------		insert into _aac_cr_load_b (ApplicationType,ReceiptDate,BankCode,ReceiptType,CheckNum,DrawnBy,Payor,Client,MatterID,InvoiceNo,
--------		                          Amt,BatchID,LineSeq,TranSeq,TranLineSeq,
--------								  _SourceFile,_ImportStatus,_StagingUser,_StagingDate,_ImportDate,TranRef)
--------		select 'M',ReceiptDate,BankCode,ReceiptType,CheckNum,DrawnBy,Payor,Client,'0001001',InvoiceNo,
--------		                          cr.amt - bm.total_ar ,BatchID,LineSeq,TranSeq,TranLineSeq,
--------								  _SourceFile,_ImportStatus,_StagingUser,_StagingDate,_ImportDate,TranRef
--------		from _aac_cr_load_b cr
--------		join hbm_matter m on cr.client = m.client_code and cr.MatterID = m.matter_code
--------		join blt_bill bl on cr.invoiceNo = bl.bill_num
--------		join blt_billm bm on bl.tran_uno = bm.bill_tran_uno and bm.matter_uno = m.matter_uno and bm.AR_status = 'O'
--------		where applicationtype = 'I' and cr.batchid = @BatchID and isnull( matterid ,'') <> ''
--------		and cr.Amt > bm.TOTAL_AR
--------						Set @Rowcount = @@Rowcount
--------				IF @DEBUG = 1 PRINT @step + ' M Rows Inserted:' + convert(varchar(10),@rowcount) 


--------		--Update 'I' row so amt = ar amount
--------		update cr set amt = bm.total_ar
--------		from _aac_cr_load_b cr
--------		join hbm_matter m on cr.client = m.client_code and cr.MatterID = m.matter_code
--------		join blt_bill bl on cr.invoiceNo = bl.bill_num
--------		join blt_billm bm on bl.tran_uno = bm.bill_tran_uno and bm.matter_uno = m.matter_uno and bm.AR_status = 'O'
--------		where applicationtype = 'I' and cr.batchid = @BatchID and isnull( matterid ,'') <> ''
--------		and cr.Amt > bm.TOTAL_AR
--------		Set @Rowcount = @@Rowcount
--------		IF @DEBUG = 1 PRINT @step + ' Rows Updated:' + convert(varchar(10),@rowcount) 


/*
	todo: validation checking
	done: tranID int		make sure this has not been converted before
	,done: crDate varchar(20)	make sure this is a valid date
	,done: GLAccount varchar(100)	make sure this is a valid GL Account
	,todo: CheckNum  varchar(50)	make sure this matches the expected format
	,todo: VendorName varchar(500) hmmmmm
	,done: ClientNum varchar(20)	make sure this is a valid client_code
	,done: MatterNum varchar(20)	make sure this is a valid matter_code on the client_code
	,todo: ExpenseCategory varchar(50)	make sure this is a valid....
	,done: InvoiceNum varchar(20)	 make sure this is a valid invoice, also check to make sure that this matter is on that bill (warn otherwise) or at least the same client (warn otherwise)
	,done: Currency varchar(10)	make sure this is a valid currency
	,done: vcCost varchar(20)	make sure this is a money amount
	,done: vcVAT			make sure this is a money amount
	,done: vcAmount		make sure this is a money amount
*/

-- 3/23/2021: placed isnull() on validate check for client.matter
-- 3/23/2021: added mail notification at the end of the proc

		---- Check that TranID has not been converted successfully before
		--insert into _AacCRImpErrorLog (Severity, BatchID, Phase, ErrorCode, TrackingNumber, IdentifierCode, ErrorValue, ErrorDesc )
		--select Severity			= 1
		--	, BatchID			= @batchID
		--	, Phase				= 'VALIDATE'
		--	, ErrorCode			= 'TranID already imported'
		--	, TrackingNumber	= a.TranID
		--	, IdentifierCode	= a.TranID
		--	, ErrorValue		= 'TranID was previously converted on batch: '+(select convert(varchar(10), max(BatchID)) from _aac_CR_success sub where sub.TranID = a.TranID)
		--	, ErrorDesc			= 'Incoming transactions identified by TranID can only be imported once.'
		--from _aac_cr_load_b a
		--WHERE exists (select * from _aac_CR_success sub where sub.TranID = a.TranID)
		--;

		-- Check that ReceiptDate is a valid date format (mm/dd/yyyy)
		insert into _AacCRImpErrorLog (Severity, BatchID, Phase, ErrorCode, TrackingNumber, IdentifierCode, ErrorValue, ErrorDesc )
		select Severity			= 1
			, BatchID			= @batchID
			, Phase				= 'VALIDATE'
			, ErrorCode			= 'ReceiptDate bad format'
			, TrackingNumber	= a.LineSeq
			, IdentifierCode	= a.ReceiptDate
			, ErrorValue		= 'ReceiptDate format must be MM/DD/YYYY or YY'
			, ErrorDesc			= 'Bad ReceiptDate format.'
		from _aac_cr_load_b a
		WHERE a.BatchID = @BatchID
		and a.ApplicationType = 'R'
		and isdate(ReceiptDate) = 0
		--;

		----------------------------------
		-- Matter ID valid when required
		----------------------------------
		insert into _AacCRImpErrorLog (Severity, BatchID, Phase, ErrorCode, TrackingNumber, IdentifierCode, ErrorValue, ErrorDesc )
		select Severity			= 0 -- Changed to severity 0,  will create receipt without allocation details
			, BatchID			= @batchID
			, Phase				= 'VALIDATE'
			, ErrorCode			= 'Matter Code invalid'
			, TrackingNumber	= a.LineSeq
			, IdentifierCode	= a.Client + '.' + a.MatterID
			, ErrorValue        = 'Client.MatterID invalid or empty. Invoice=' + convert(varchar(20), A.invoiceno) + '  C.M=<' + isnull(a.client, 'NULL') + '.' + isnull(a.matterID, 'NULL') + '>'
			, ErrorDesc			= 'Client.MatterID must exist in hbm_matter.client_code/matter_code'
		from _aac_cr_load_b a
		WHERE a.BatchID = @BatchID
		and a.ApplicationType in ('I', 'T', 'Z')
		and not exists (select * from hbm_matter sub where sub.client_code = a.Client and sub.matter_code = a.MatterID)
		--;



		----------------------------------
		-- R - Receipt header rows
		----------------------------------

		-- Check that BankCode is valid
		insert into _AacCRImpErrorLog (Severity, BatchID, Phase, ErrorCode, TrackingNumber, IdentifierCode, ErrorValue, ErrorDesc )
		select Severity			=  0 -- Changed to severity 0,  will create receipt without allocation details
			, BatchID			= @batchID
			, Phase				= 'VALIDATE'
			, ErrorCode			= 'BankCode invalid'
			, TrackingNumber	= a.LineSeq
			, IdentifierCode	= a.BankCode
			, ErrorValue		= 'BankCode invalid'
			, ErrorDesc			= 'BankCode should be valid in tbm_bank.bank_code'
		from _aac_cr_load_b a
		WHERE a.BatchID = @BatchID
		and a.ApplicationType = 'R'
		and not exists (select * from tbm_bank sub where sub.bank_code = a.BankCode and sub.inactive = 'N')
		--;

		----------------------------------
		-- I - Invoice offset rows
		----------------------------------
		-- check for valid invoice
		insert into _AacCRImpErrorLog (Severity, BatchID, Phase, ErrorCode, TrackingNumber, IdentifierCode, ErrorValue, ErrorDesc )
		select Severity			=  0 -- Changed to severity 0,  will create receipt without allocation details
			, BatchID			= @batchID
			, Phase				= 'VALIDATE'
			, ErrorCode			= 'InvoiceNo invalid'
			, TrackingNumber	= a.LineSeq
			, IdentifierCode	= a.InvoiceNo
			, ErrorValue		= 'InvoiceNo invalid'
			, ErrorDesc			= 'InvoiceNo must exist in blt_bill.bill_num'
		from _aac_cr_load_b a
		WHERE a.BatchID = @BatchID
		and a.ApplicationType in ('I')
		and not exists (select * from blt_bill sub where sub.bill_num = a.InvoiceNo)
		--;

		-- check for valid invoice/matter combo
		insert into _AacCRImpErrorLog (Severity, BatchID, Phase, ErrorCode, TrackingNumber, IdentifierCode, ErrorValue, ErrorDesc )
		select Severity			=  0 -- Changed to severity 0,  will create receipt without allocation details
			, BatchID			= @batchID
			, Phase				= 'VALIDATE'
			, ErrorCode			= 'InvoiceNo/matter invalid'
			, TrackingNumber	= a.LineSeq
			, IdentifierCode	= a.InvoiceNo + '/' + a.Client + '.' + a.MatterID
			, ErrorValue		= 'InvoiceNo <' + a.invoiceno + '>/matter <' + isnull(a.client,'NULL') + '.' + isnull(a.matterID, 'NULL') + '> invalid'
			, ErrorDesc			= 'InvoiceNo/matter must exist in blt_bill_amt'
		from _aac_cr_load_b a
		WHERE a.BatchID = @BatchID
		and a.ApplicationType in ('I')
		and not exists (select *
										from blt_bill sub
										inner join blt_bill_amt sub2 on sub2.bill_tran_uno = sub.tran_uno and sub2.tran_type = 'BL'
										inner join hbm_matter sub3 ON sub3.matter_uno = sub2.matter_uno
										where sub.bill_num = a.InvoiceNo
										and sub3.client_code = a.Client and sub3.matter_code = a.MatterID
									)
		--;

		----------------------------------
		-- G - General Ledger offset rows
		----------------------------------

		-- Check that GLAccount is valid in glm_chart
		insert into _AacCRImpErrorLog (Severity, BatchID, Phase, ErrorCode, TrackingNumber, IdentifierCode, ErrorValue, ErrorDesc )
		select Severity			=  0 -- Changed to severity 0,  will create receipt without allocation details
			, BatchID			= @batchID
			, Phase				= 'VALIDATE'
			, ErrorCode			= 'GLAcct invalid'
			, TrackingNumber	= a.LineSeq
			, IdentifierCode	= a.GLAcct
			, ErrorValue		= 'GLAcct invalid'
			, ErrorDesc			= 'GLAcct should be valid in glm_chart.acct_code'
		from _aac_cr_load_b a
		WHERE a.BatchID = @BatchID
		and a.ApplicationType = 'G'
		and not exists (select * from glm_chart sub where sub.acct_code = a.BankCode)
		;

		-- Check that GLOffc is valid in hbl_office
		insert into _AacCRImpErrorLog (Severity, BatchID, Phase, ErrorCode, TrackingNumber, IdentifierCode, ErrorValue, ErrorDesc )
		select Severity			= 1
			, BatchID			= @batchID
			, Phase				= 'VALIDATE'
			, ErrorCode			= 'GLOffc invalid'
			, TrackingNumber	= a.LineSeq
			, IdentifierCode	= a.GLOffc
			, ErrorValue		= 'GLOffc invalid'
			, ErrorDesc			= 'GLOffc should be valid in hbl_office.offc_code'
		from _aac_cr_load_b a
		WHERE a.BatchID = @BatchID
		and a.ApplicationType = 'G'
		and not exists (select * from hbl_office sub where sub.offc_code = a.GLOffc)
		;

		-- Check that GLDept is valid in hbl_dept
		insert into _AacCRImpErrorLog (Severity, BatchID, Phase, ErrorCode, TrackingNumber, IdentifierCode, ErrorValue, ErrorDesc )
		select Severity			= 1
			, BatchID			= @batchID
			, Phase				= 'VALIDATE'
			, ErrorCode			= 'GLDept invalid'
			, TrackingNumber	= a.LineSeq
			, IdentifierCode	= a.GLDept
			, ErrorValue		= 'GLDept invalid'
			, ErrorDesc			= 'GLDept should be valid in hbl_dept.dept_code'
		from _aac_cr_load_b a
		WHERE a.BatchID = @BatchID
		and a.ApplicationType = 'G'
		and not exists (select * from hbl_dept sub where sub.dept_code = a.GLDept)
		;

		-- -- Check for valid ClientNum
		-- insert into _AacCRImpErrorLog (Severity, BatchID, Phase, ErrorCode, TrackingNumber, IdentifierCode, ErrorValue, ErrorDesc )
		-- select 1 as 'Severity'
		-- 	, @batchID	as 'BatchID'
		-- 	, 'CR' as 'Phase'
		-- 	, 'ClientNum not found' as 'ErrorCode'
		-- 	, a.CheckNum as 'TrackingNumber'
		-- 	, a.ClientNum as 'IdentifierCode'
		-- 	, 'ClientNum not found' as 'ErrorValue'
		-- 	, 'The ClientNum must be a valid client found in hbm_client.client_code'
		-- from _aac_cr_load_b a
		-- WHERE isnull(a.ClientNum, '') <> ''
		-- and not exists ( select * from hbm_client sub where sub.client_code = a.ClientNum)
		-- ;

		---- Check for valid ClientNum and MatterNum
		--insert into _AacCRImpErrorLog (Severity, BatchID, Phase, ErrorCode, TrackingNumber, IdentifierCode, ErrorValue, ErrorDesc )
		--select 1 as 'Severity'
		--	, @batchID	as 'BatchID'
		--	, 'CR' as 'Phase'
		--	, 'ClientNum/MatterNum not found' as 'ErrorCode'
		--	, a.CheckNum as 'TrackingNumber'
		--	, isnull(a.ClientNum, '') + '.' + isnull(a.MatterNum, '') as 'IdentifierCode'
		--	, 'ClientNum.MatterNum not found' as 'ErrorValue'
		--	, 'The ClientNum.MatterNum must be a valid client.matter found in hbm_matter.matter_code'
		--from _aac_cr_load_b a
		--WHERE isnull(a.MatterNum, '') <> ''
		--and not exists ( select * from hbm_matter sub where sub.client_code = a.ClientNum and sub.matter_code = a.MatterNum)
		--;

		---- Check that InvoiceNum is valid in blt_bill.bill_num
		--insert into _AacCRImpErrorLog (Severity, BatchID, Phase, ErrorCode, TrackingNumber, IdentifierCode, ErrorValue, ErrorDesc )
		--select Severity			= 1
		--	, BatchID			= @batchID
		--	, Phase				= 'VALIDATE'
		--	, ErrorCode			= 'InvoiceNum Invalid'
		--	, TrackingNumber	= a.CheckNum
		--	, IdentifierCode	= a.InvoiceNum
		--	, ErrorValue		= 'InvoiceNum Invalid'
		--	, ErrorDesc			= 'InvoiceNum should be found in blt_bill.num'
		--from _aac_cr_load_b a
		--WHERE not exists (select * from blt_bill sub where sub.bill_num = try_convert(int, a.InvoiceNum))
		--AND a.InvoiceNum is not null
		--;

		---- Check that InvoiceNum/ClientNum/MatterNum matches is the Aderant bill
		--insert into _AacCRImpErrorLog (Severity, BatchID, Phase, ErrorCode, TrackingNumber, IdentifierCode, ErrorValue, ErrorDesc )
		--select Severity			= 1
		--	, BatchID			= @batchID
		--	, Phase				= 'VALIDATE'
		--	, ErrorCode			= 'InvoiceNum.ClientNum.MatterNum not matching'
		--	, TrackingNumber	= a.CheckNum
		--	, IdentifierCode	= isnull(a.InvoiceNum, '') + '.' + isnull(a.ClientNum, '') + '.' +  isnull(a.MatterNum, '')
		--	, ErrorValue		= 'InvoiceNum.ClientNum.MatterNum not matching'
		--	, ErrorDesc			= 'InvoiceNum.ClientNum.MatterNum should match the client/matter on the bill in Aderant'
		--from _aac_cr_load_b a
		--WHERE a.InvoiceNum is not null
		--	and not exists (select * from blt_bill sub
		--	inner join blt_billm sub2 on sub2.bill_tran_uno = sub.tran_uno
		--	inner join hbm_matter sub3 on sub3.matter_uno = sub2.matter_uno
		--	inner join blt_billp sub4 on sub4.bill_tran_uno = sub.tran_uno
		--	inner join hbm_client sub5 on sub5.client_uno = sub4.payr_client_uno
		--	where sub.bill_num = CASE WHEN isnull(try_convert(int, a.InvoiceNum), -999) <> -999 THEN a.InvoiceNum ELSE sub.bill_num END
		--	--and sub5.CLIENT_CODE = CASE WHEN isnull(a.ClientNum, '') <> '' THEN a.ClientNum ELSE sub5.CLIENT_CODE END
		--	and sub3.MATTER_CODE = CASE WHEN isnull(a.MatterNum, '') <> '' THEN a.MatterNum ELSE sub3.MATTER_CODE END
		--	)
		--;

		---- Check that Currency is valid in tbl_currency
		--insert into _AacCRImpErrorLog (Severity, BatchID, Phase, ErrorCode, TrackingNumber, IdentifierCode, ErrorValue, ErrorDesc )
		--select Severity			= 20
		--	, BatchID			= @batchID
		--	, Phase				= 'VALIDATE'
		--	, ErrorCode			= 'Currency invalid'
		--	, TrackingNumber	= a.TranID
		--	, IdentifierCode	= a.Currency
		--	, ErrorValue		= 'Currency invalid'
		--	, ErrorDesc			= 'Currency value in IdentifierCode should be found in tbl_currency.'
		--from _aac_cr_load_b a
		--WHERE not exists (select * from glm_chart sub where sub.acct_code = a.GLAccount)
		--;

		---- Check that vcCost is a valid currency format
		--insert into _AacCRImpErrorLog (Severity, BatchID, Phase, ErrorCode, TrackingNumber, IdentifierCode, ErrorValue, ErrorDesc )
		--select Severity			= 1
		--	, BatchID			= @batchID
		--	, Phase				= 'VALIDATE'
		--	, ErrorCode			= 'vcCost bad format'
		--	, TrackingNumber	= a.TranID
		--	, IdentifierCode	= a.vcCost
		--	, ErrorValue		= 'vcCost bad format'
		--	, ErrorDesc			= 'vcCost format may only contain digits, commas and two decimals.'
		--from _aac_cr_load_b a
		--WHERE vcCost like '%[^0123456789.,]%'
		--;

		---- Check that vcVAT is a valid currency format
		--insert into _AacCRImpErrorLog (Severity, BatchID, Phase, ErrorCode, TrackingNumber, IdentifierCode, ErrorValue, ErrorDesc )
		--select Severity			= 1
		--	, BatchID			= @batchID
		--	, Phase				= 'VALIDATE'
		--	, ErrorCode			= 'vcVAT bad format'
		--	, TrackingNumber	= a.TranID
		--	, IdentifierCode	= a.vcVAT
		--	, ErrorValue		= 'vcVAT bad format'
		--	, ErrorDesc			= 'vcVAT format may only contain digits, commas and two decimals.'
		--from _aac_cr_load_b a
		--WHERE vcVAT like '%[^0123456789.,]%'
		--;

		---- Check that vcAmount is a valid currency format
		--insert into _AacCRImpErrorLog (Severity, BatchID, Phase, ErrorCode, TrackingNumber, IdentifierCode, ErrorValue, ErrorDesc )
		--select Severity			= 1
		--	, BatchID			= @batchID
		--	, Phase				= 'VALIDATE'
		--	, ErrorCode			= 'vcAmount bad format'
		--	, TrackingNumber	= a.TranID
		--	, IdentifierCode	= a.vcAmount
		--	, ErrorValue		= 'vcAmount bad format'
		--	, ErrorDesc			= 'vcAmount format may only contain digits, commas and two decimals.'
		--from _aac_cr_load_b a
		--WHERE vcAmount like '%[^0123456789.,]%'
		--;


		----------------------
		-- BEGIN EMAIL SECTION
		----------------------
		DECLARE @tableHTML  NVARCHAR(MAX)
		DECLARE @MailRecipients [nvarchar](max), @CCMailRecipients [nvarchar](max), @MailNotifyOnSuccess varchar(1)
		DECLARE @profile_to_use nvarchar(500)

		set @profile_to_use = 'svc_aderant_smtp'  --FIRM-DEPENDENT SETTING


		select @MailRecipients = MailRecipients, @CCMailRecipients = CCMailRecipients, @MailNotifyOnSuccess = MailNotifyOnSuccess from [_AacCRImpParmSys] a

		-- if this is true, an error will be raised and the rest of the SP will be skipped
		if ( ( select count(*) from _AacCRImpErrorLog a where a.BatchID = @batchID and a.Severity < ( select SeverityThreshold from _AacCRImpParmSys ) )
			> ( select ErrorThreshold from _AacCRImpParmSys ) )
		   AND (@MailRecipients is not null OR @CCMailRecipients is not null )
		begin

			SET @tableHTML =
			    N'<p>The number of errors that occurred during the batch load exceeded the allowable threshold and the process was rolled back.  No data has been loaded.</p>' +
					N'<p>Input file: ' + (select filepath from _aaccrimpbatch sub where sub.batchid = @BatchID) + '</p>' +
			    N'<table border="1">' +
			    N'<tr><th>Severity</th><th>BatchID</th><th>Phase</th><th>ErrorCode</th><th>TrackingNumber</th><th>IdentifierCode</th><th>ErrorValue</th><th>ErrorDesc</th></tr>' +
			    CAST ( ( SELECT td = a.Severity,       ''
					,td = a.BatchID,       ''
					,td = a.Phase,       ''
					,td = a.ErrorCode,       ''
					,td = a.TrackingNumber,       ''
					,td = a.IdentifierCode,       ''
					,td = a.ErrorValue,       ''
					,td = a.ErrorDesc,       ''
				      FROM _AacCRImpErrorLog as a
					WHERE a.BatchID = @batchID
				      FOR XML PATH('tr'), TYPE
			    ) AS NVARCHAR(MAX) ) +
			    N'</table>' ;

-- todo, need to add permissions for sp_send_dbmail
--			exec msdb.dbo.sp_Send_dbmail
--				@profile_name = @profile_to_use
--				,@recipients = @MailRecipients
--				,@copy_recipients = @CCMailRecipients
--				,@body = @tableHTML
--				,@body_format = 'HTML'
--				,@subject = 'Import batch aborted due to errors'
--				,@importance = 'High';

		end
		----------------------
		-- END EMAIL SECTION
		----------------------

----
	EXEC SpAacCRLogProcedure @ObjectID = @@PROCID, @BatchID = @batchID, @ProcedurePhase = 'END'

END TRY
BEGIN CATCH

	if @@trancount > 0 rollback transaction
	EXEC SpAacCRRaisError;

	EXEC SpAacCRLogProcedure @ObjectID = @@PROCID, @BatchID = @batchID, @ProcedurePhase = 'ERR';

END CATCH
set nocount off


GO


