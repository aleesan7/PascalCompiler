program intermedios;
    var compare : integer = 56;
    var last : integer = 2;
    var index : real = 0;
    var i: integer = 0;
    var tabla: integer = 0;
	var numero: integer = 0;
begin
    writeln('***********************************************************************');
    writeln('***********                 IF                         ****************');
    writeln('***********************************************************************');

    if (compare > 50) then
	begin
		writeln('IF CORRECTO');
	end
    else 
	begin
		if (compare = 56) then
		begin
			writeln('IF INCORRECTO');
		end
		else
		begin
			writeln('IF INCORRECTO');
		end
	end
	
    writeln('***********************************************************************');
    writeln('***********                 SWITCH                     ****************');
    writeln('***********************************************************************');

    case last of
        1:
            begin
                writeln('SWITCH MALO');        
            end;
        2:
            begin
                writeln('SWITCH BIEN');
            end;
        3:
            begin
                writeln('SWITCH MALO');
            end;
        else
            begin
                writeln('SWITCH MALO');
            end;
    end;


    case last of
        1:
            begin
                writeln('SWITCH MALO');        
            end;
        -2:
            begin
                writeln('SWITCH MALO');
            end;
        3:
            begin
                writeln('SWITCH MALO');
            end;
        else
            begin
                writeln('SWITCH BIEN');
            end;
    end;
    
    writeln('***********************************************************************');
    writeln('***********                 WHILE                      ****************');
    writeln('***********************************************************************');

    while index >= 0 do
    begin
        if (index = 0) then
		begin
			index := index + 100;
		end
        else 
		begin
			if (index > 50) then
			begin
				index := index / 2 - 25;
			end
			else
			begin
				index := (index / 2) - 1;
			end
		end
		
        writeln(index);
    end;

    writeln('***********************************************************************');
    writeln('************                 REPEAT                    ****************');
    writeln('***********************************************************************');
    i := -1;
    repeat
        i := i + 1;
        if ( (i = 0) OR (i = 1) OR (i = 11) OR (i = 12) ) then
		begin
			writeln('*********************************************************************************************************');
		end
        else 
		begin
			if (i = 2) then
			begin
				writeln('**********  ***************  ******                 ******                 ******              **********');
			end
			else 
			begin
				if ((i >= 3) AND (i <= 5)) then
				begin
					writeln('**********  ***************  ******  *********************  *************  ******  **********************');
				end
				else 
				begin
					if (i = 6) then
					begin
						writeln('**********  ***************  ******                 ******                 ******  **********************');
					end
					else 
					begin
						if ((i >= 7) AND (i <= 9)) then
						begin
							writeln('**********  ***************  ********************   ******  *************  ******  **********************');
						end    
						else 
						begin
							if (i = 10) then
							begin
								writeln('**********                   ******                 ******  *************  ******              **********');
							end
						end
					end
				end
			end
		end
		
		
		
    until i > 12;

    for tabla := 1 to 5 do
    begin
        for numero := 1 to 10 do
        begin
            write( tabla);
			write(' por ');
			write(numero); 
			write(' es ');
			writeln(tabla * numero);
        end;
        writeln('');        
    end;

end.