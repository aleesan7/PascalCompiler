program Funciones;
function factorial(n: integer): integer;
begin
    if (n = 0) then
	begin
        Exit(1);
	end
    else
	begin
        Exit(n * factorial(n - 1));
	end
end;

function ackermann(m:integer; n: integer): integer;
begin
    if (m = 0) then
	begin
        Exit(n + 1);
	end
    else 
	begin
		if ((m>0) AND (n = 0)) then
		begin
			Exit(ackermann(m - 1, 1));
		end
		else
		begin
			Exit(ackermann(m - 1, ackermann(m,n - 1)));
		end
	end
end;

procedure Hanoi(discos:integer; origen: string; aux: string; destino:string);
begin
    if(discos=1) then
	begin
        write('Mover Disco de ');
		write(origen);
		write(' a ');
		writeln(destino);
	end
    else
	Begin
		Hanoi(discos-1,origen,destino,aux);
		write('Mover disco de ');
		write(origen);
		write(' a ');
		writeln(destino);
		Hanoi(discos-1,aux,origen,destino);
	End
end;

begin
    writeln('1 Factorial');
    writeln(factorial(6));

    writeln('2 Ackermann');
    writeln(ackermann(3,4));
    
    writeln('3 Hanoi');
    Hanoi(3, 'A', 'B', 'C');
end.