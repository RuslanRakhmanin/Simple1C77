cd %~dp0
%~d0
for %%f in (*.dot) do (
    echo %%~nf
	dot -Tpng -o "%%~nf.png" "%%~nf.dot"
)
pause