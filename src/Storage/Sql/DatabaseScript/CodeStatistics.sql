IF object_id('tempdb..#SessionTable') IS NOT NULL BEGIN 
DROP TABLE #SessionTable
END


SELECT 
		r.session_id AS spid
        ,r.cpu_time,r.reads,r.writes,r.logical_reads 
        ,r.blocking_session_id AS BlockingSPID
        ,LEFT(OBJECT_NAME(st.objectid, st.dbid),50) AS ShortObjectName
        ,LEFT(DB_NAME(r.database_id),50) AS DatabaseName
        ,s.program_name
        ,s.login_name
        ,OBJECT_NAME(st.objectid, st.dbid) AS ObjectName
		,s.last_request_start_time
		,s.last_request_end_time
        ,r.command
		,r.wait_type
		,r.last_wait_type
		,transaction_isolation_level = CASE r.transaction_isolation_level 
			WHEN 1 THEN 'ReadUncomitted'
			WHEN 2 THEN 'ReadCommitted'
			WHEN 3 THEN 'Repeatable'
			WHEN 4 THEN 'Serializable'
			WHEN 5 THEN 'Snapshot' ELSE 'Unspecified' END

		--,SUBSTRING(st.text, (r.statement_start_offset/2)+1,( (CASE r.statement_end_offset
        --                                                          WHEN -1 THEN DATALENGTH(st.text)
        --                                                          ELSE r.statement_end_offset
        --                                                      END - r.statement_start_offset
        --                                                     )/2
        --                                                   ) + 1
        --          ) AS SQLText
		--,qp.query_plan
		,tl.resource_description
		,tl.request_mode
		,tl.request_type
		,tl.request_lifetime
		,tl.request_owner_type
INTO #SessionTable
    FROM sys.dm_exec_requests r
			JOIN sys.dm_exec_sessions  s ON r.session_id = s.session_id
				CROSS APPLY sys.dm_exec_sql_text (sql_handle) st
					CROSS APPLY sys.dm_exec_query_plan (plan_handle) qp
						JOIN sys.dm_tran_locks tl ON tl.request_session_id = r.session_id
	WHERE request_owner_type = 'transaction'
	--AND r.session_id <>@@spid

--SELECT 
--	st.spid
--	,st.BlockingSPID
--	,st.command
--	,st.request_mode
--	,st.request_owner_type
--	,st.last_request_start_time
--	,transaction_isolation_level
--	,COUNT(*) 
--FROM 
--	#SessionTable st
--GROUP BY
--	spid
--	,st.BlockingSPID 
--	,st.command
--	,st.last_request_start_time
--	,transaction_isolation_level
--	,st.request_mode
--	,st.request_owner_type
--ORDER BY 1


select 
	* 
FROM 
	#SessionTable st