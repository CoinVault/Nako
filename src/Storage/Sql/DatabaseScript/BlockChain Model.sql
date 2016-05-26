/****** Denormalized model ******/


IF OBJECT_ID('Blocks.Block') is not null begin 
drop table Blocks.Block
end

CREATE TABLE [Blocks].[Block](
	[BlockIndex]		[bigint] NOT NULL,
	[BlockHash]			[varchar](200) NULL,
	[NextBlockHash]		[varchar](200) NULL,
	[PreviousBlockHash] [varchar](200) NOT NULL)

--Block
--insert into [Blocks].[Block]
--SELECT 
--	[BlockIndex]
--	,[BlockHash]
--	,[NextBlockHash]
--	,[PreviousBlockHash]
--FROM 
--	[dbo].[Blocks]


IF OBJECT_ID('Blocks.BlockDetail') is not null begin 
drop table Blocks.BlockDetail
end

CREATE TABLE [Blocks].[BlockDetail] (
	[BlockIndex] [bigint] NOT NULL,
	[BlockTime] [bigint] NOT NULL,
	[SyncComplete] [bit] NOT NULL,
	[TransactionCount] [int] NOT NULL)

--BlockDetails	
--insert into Blocks.BlockDetail
--SELECT 
--	[BlockIndex]
--	,[BlockTime]
--	,[SyncComplete]
--	,[TransactionCount]
--FROM 
--	[dbo].[Blocks];


IF OBJECT_ID('Blocks.BlockTransaction') is not null begin 
drop table Blocks.BlockTransaction
end

CREATE TABLE [Blocks].[BlockTransaction]  (
	[TransactionId] bigint NOT NULL,
	[BlockIndex] [bigint] NOT NULL
	--[TransactionCode] varchar(200) NOT NULL,
	--[Timestamp] datetime2(7) NOT NULL
	)

--BlockTrasaction
--insert into Blocks.BlockTransaction
--SELECT 
--	TransactionId = ROW_NUMBER()OVER(ORDER BY BlockIndex, TransactionId)
--	,BlockIndex
--	,TransactionCode = TransactionId
--    ,Timestamp
--FROM 
--	[dbo].[BlockTransactions];


--if OBJECT_ID('tempdb..#TrnInOut') is not null begin 
--drop table #TrnInOut
--end 

--create table #TrnInOut (
--	[TransactionChainId] bigint NOT NULL,
--	[TransactionId] bigint NOT NULL,
--	[Index] [int] NOT NULL,
--	[IndexType] [int] NOT NULL,
--	[InputCoinBase] [varchar](200) NULL,
--	[InputIndex] [int] NULL,
--	[InputTransactionId] [varchar](200) NULL,
--	[InputAddress] [varchar](200) NULL,
--	[InputValue] [decimal](18, 8) NULL,
--	[OutputAddress] [varchar](200) NULL,
--	[OutputType] [varchar](32) NULL,
--	[OutputValue] [decimal](18, 8) NULL,
--	[OutputSpent] [bit] NULL,
--	[OutputSpentTransactionId] [varchar](200) NULL)

--insert into #TrnInOut

--SELECT 
--	TransactionChainId = ROW_NUMBER()over(order by a.[TransactionId], [Index] ,[IndexType])
--	,a.[TransactionId]
--	,[Index]
--	,[IndexType]
--	,t.InputCoinBase
--	,t.InputIndex
--	,t.InputTransactionId
--	,t.InputAddress
--	,t.InputValue
--	,t.OutputAddress
--	,t.OutputType
--	,t.OutputValue
--	,t.OutputSpent
--	,t.OutputSpentTransactionId
--FROM 
--	[dbo].[Transactions] t
--	INNER JOIN [Blocks].[BlockTransaction] a ON t.TransactionId = a.TransactionCode





--IF OBJECT_ID('Chains.BlockChainTransaction') is not null begin 
--drop table Chains.BlockChainTransaction
--end

--create table Chains.BlockChainTransaction (
--	TransactionChainId bigint not null
--	,TransactionId bigint not null
--)

--insert into Chains.BlockChainTransaction
--SELECT
--	[TransactionChainId]
--	,TransactionId
--FROM
--	#TrnInOut



IF OBJECT_ID('Chains.[Transaction]') is not null begin 
drop table Chains.[Transaction]
end


CREATE TABLE Chains.[Transaction] (
	[TransactionId] bigint IDENTITY(1,1) NOT NULL,
	[TransactionTime] [bigint] NOT NULL,
	TransactionHash varchar(200) NOT NULL,
)

--insert into Chains.TransactionChain

--SELECT
--	[TransactionChainId]
--	,InputIndex
--	,InputTransactionId
--FROM
--	#TrnInOut
--WHERE 
--	InputTransactionId is not null


IF OBJECT_ID('Chains.InputCoinBase') is not null begin 
drop table Chains.InputCoinBase
end

CREATE TABLE Chains.InputCoinBase (
	[TransactionId] bigint NOT NULL,
	InputCoinBase varchar(200) NOT NULL,
)

--insert into Chains.InputCoinBase

--SELECT
--	[TransactionChainId]
--	,InputCoinBase
--FROM
--	#TrnInOut
--WHERE 
--	InputCoinBase is not null; 


IF OBJECT_ID('Chains.Input') is not null begin 
drop table Chains.Input
end

CREATE TABLE Chains.Input (
	[TransactionId] bigint NOT NULL,
	[PreviousTransactionId] bigint NOT NULL,
	[PreviousIndex] bigint NOT NULL,
	InputAddress varchar(200) NULL, -- how do we populate this (do we need to?)
	InputValue decimal(18, 8) NULL, -- how do we populate this (do we need to?)
)

--insert into Chains.InputAddressValue

--SELECT
--	[TransactionChainId]
--	,InputAddress
--	,InputValue
--FROM
--	#TrnInOut
--WHERE 
--	InputValue is not null 
	

IF OBJECT_ID('Chains.[Output]') is not null begin 
drop table Chains.[Output]
end

CREATE TABLE Chains.[Output] (
	[TransactionId] bigint NOT NULL,
	[Index] int NOT NULL,
	OutputAddress varchar(200) NOT NULL,
	[Type] varchar(32) NOT NULL,
	Value decimal(18,8) NOT NULL,
    Spent bit not null, -- Do we need this
)

--insert into Chains.OutputAddressValue
--SELECT
--	[TransactionChainId]
--	,OutputIndex = [Index]
--	,OutputAddress
--	,OutputType
--	,OutputValue
--FROM
--	#TrnInOut
--WHERE 
--	OutputAddress is not null 


--IF OBJECT_ID('Chains.OutputSpent') is not null begin 
--drop table Chains.OutputSpent
--end

--CREATE TABLE Chains.OutputSpent (
--	[TransactionChainId] int NOT NULL,
--	OutputIndex int not null,
--	OutputSpent decimal(18,8) not null,
--	OutputSpentTransactionId VARCHAR(200) not null
--)

--insert into Chains.OutputSpent

--SELECT
--	[TransactionChainId]
--	,OutputIndex = [Index]
--	,OutputSpent
--	,OutputSpentTransactionId
--FROM
--	#TrnInOut
--WHERE 
--	OutputSpentTransactionId is not null 