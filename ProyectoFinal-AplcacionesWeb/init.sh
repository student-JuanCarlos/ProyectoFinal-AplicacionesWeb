#!/bin/bash
echo "Esperando SQL Server..."
sleep 20
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P ${PasswordSa} -i /init.sql
echo "BD inicializada!"