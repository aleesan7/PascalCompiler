using Irony.Parsing;
using Irony.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace PascalCompiler.Analysers
{
    class CompilerGrammar : Grammar
    {
        public CompilerGrammar() : base(caseSensitive: false)
        {
            #region ER
            CommentTerminal lineComment = new CommentTerminal("lineComment", "//", "\n", "\r\n"); //si viene una nueva linea se termina de reconocer el comentario.
            CommentTerminal blockComment = new CommentTerminal("blockComment", "(*", "*)");
            CommentTerminal multiLineComment = new CommentTerminal("multiLineComment", "{", "}");

            IdentifierTerminal identifier = new IdentifierTerminal("identifier");
            StringLiteral STRING = new StringLiteral("string", "\'");
            var INTEGER = new NumberLiteral("integer", NumberOptions.AllowSign);
            var REAL = new RegexBasedTerminal("real", "[0-9]+[.][0-9]+");

            #endregion

            #region Terminals
            var ARRAY = ToTerm("array");
            var INTEGERtype = ToTerm("integer");
            var STRINGType = ToTerm("string");
            var REALType = ToTerm("real");
            var BOOLEANtype = ToTerm("boolean");

            var AND = ToTerm("AND");
            var NOT = ToTerm("NOT");
            var OR = ToTerm("OR");
            var MINUS = ToTerm("-");
            var STAR = ToTerm("*");
            var PLUS = ToTerm("+");
            var DIV = ToTerm("/");
            var MOD = ToTerm("%");

            var FUNCTION = ToTerm("function");
            var PROCEDURE = ToTerm("procedure");

            var OF = ToTerm("of");
            var DO = ToTerm("do");
            var TO = ToTerm("to");
            var DOWNTO = ToTerm("downto");
            var DIVKEYWORKD = ToTerm("div");
            var ELSE = ToTerm("else");
            var END = ToTerm("end");
            var FOR = ToTerm("for");
            var IF = ToTerm("if");
            var BEGIN = ToTerm("begin");
            var PROGRAM = ToTerm("program");
            var CONST = ToTerm("const");
            var REPEAT = ToTerm("repeat");
            var THEN = ToTerm("then");
            var TYPE = ToTerm("type");
            var UNTIL = ToTerm("until");
            var VAR = ToTerm("var");
            var WHILE = ToTerm("while");
            var CASE = ToTerm("case");
            var EXIT = ToTerm("exit");
            var WRITE = ToTerm("write");
            var WRITELN = ToTerm("writeln");
            var GRAPHTS = ToTerm("graficar_ts");
            var BREAK = ToTerm("break");
            var CONTINUE = ToTerm("continue");

            var SEMICOLON = ToTerm(";");
            var EQUAL = ToTerm("=");
            var COLON = ToTerm(":");
            var COMMA = ToTerm(",");
            var DOT = ToTerm(".");
            var DOUBLEDOT = ToTerm("..");
            var LEFTBRAC = ToTerm("[");
            var RIGHTBRAC = ToTerm("]");
            var LEFTPAREN = ToTerm("(");
            var RIGHTPAREN = ToTerm(")");
            var NOTEQUAL = ToTerm("<>");
            var GREATER = ToTerm(">");
            var GREATHEROREQUAL = ToTerm(">=");
            var SMALLERTHAN = ToTerm("<");
            var SMALLEROREQUAL = ToTerm("<=");

            var TRUE = ToTerm("true", "boolean");
            var FALSE = ToTerm("false", "boolean");

            RegisterOperators(1, PLUS, MINUS);
            RegisterOperators(2, STAR, DIV);

            NonGrammarTerminals.Add(lineComment);
            NonGrammarTerminals.Add(blockComment);
            NonGrammarTerminals.Add(multiLineComment);
            #endregion

            #region Non Terminals
            NonTerminal program = new NonTerminal("program");
            NonTerminal program_heading = new NonTerminal("program_heading");
            NonTerminal block = new NonTerminal("block");
            NonTerminal constant_definition_part = new NonTerminal("constant_definition_part");
            NonTerminal constant_definition = new NonTerminal("constant_definition");
            NonTerminal type_definition_part = new NonTerminal("type_definition_part");
            NonTerminal variable_definition_part = new NonTerminal("variable_definition_part");
            NonTerminal variable_definition = new NonTerminal("variable_definition");
            NonTerminal procedure_and_function_declaration_part = new NonTerminal("procedure_and_function_declaration_part");
            NonTerminal proc_or_func_declaration_list = new NonTerminal("proc_or_func_declaration_list");
            NonTerminal proc_or_func_declaration = new NonTerminal("proc_or_func_declaration");
            NonTerminal procedure_declaration = new NonTerminal("procedure_declaration");
            NonTerminal function_declaration = new NonTerminal("function_declaration");
            NonTerminal const_values = new NonTerminal("const_values");
            NonTerminal tipo_funcion = new NonTerminal("tipo_funcion");
            NonTerminal parametros = new NonTerminal("parametros");
            NonTerminal parametro = new NonTerminal("parametro");
            NonTerminal instrucciones = new NonTerminal("instrucciones");
            NonTerminal declaracion = new NonTerminal("declaracion");
            NonTerminal declaracion_array = new NonTerminal("declaracion_array");
            NonTerminal dimensiones = new NonTerminal("dimensiones");
            NonTerminal expresion = new NonTerminal("expresion");
            NonTerminal expresiones = new NonTerminal("expresiones");
            NonTerminal instruccion = new NonTerminal("instruccion");
            NonTerminal instruccion_if_sup = new NonTerminal("instruccion_if_sup");
            NonTerminal instruccion_if = new NonTerminal("instruccion_if");
            NonTerminal instrucciones_elseif = new NonTerminal("instrucciones_elseif");
            NonTerminal instruccion_else = new NonTerminal("instruccion_else");
            NonTerminal instruccion_elseif = new NonTerminal("instruccion_elseif");
            NonTerminal tipo = new NonTerminal("tipo");
            NonTerminal lista_identificadores = new NonTerminal("lista_identificadores");
            NonTerminal functionOrProcedureCall = new NonTerminal("functionOrProcedureCall");
            NonTerminal case_statement = new NonTerminal("case_statement");
            NonTerminal case_elements = new NonTerminal("case_elements");
            NonTerminal case_element = new NonTerminal("case_element");
            NonTerminal ranges_list = new NonTerminal("ranges_list");
            NonTerminal range = new NonTerminal("range");

            #endregion

            #region Grammar
            program.Rule = program_heading + SEMICOLON + block + DOT;

            program_heading.Rule = PROGRAM + identifier;


            block.Rule = constant_definition_part + //type_definition_part +
                     variable_definition_part + procedure_and_function_declaration_part +
                     BEGIN + instrucciones + END;
            block.ErrorRule = SyntaxError + END
                                  | SyntaxError + SEMICOLON;

            constant_definition_part.Rule = MakePlusRule(constant_definition_part, constant_definition)
                                   | Empty;

            constant_definition.Rule = CONST + declaracion + EQUAL + const_values + SEMICOLON;

            const_values.Rule = INTEGER
                              | STRING
                              | REAL
                              | TRUE
                              | FALSE;

            variable_definition_part.Rule = MakePlusRule(variable_definition_part, variable_definition)
                                   | Empty;

            variable_definition.Rule = VAR + lista_identificadores + COLON + tipo + SEMICOLON
                               | VAR + identifier + COLON + tipo + EQUAL + expresion + SEMICOLON;
            variable_definition.ErrorRule = SyntaxError + SEMICOLON;

            procedure_and_function_declaration_part.Rule = proc_or_func_declaration_list
                                                  | Empty;

            proc_or_func_declaration_list.Rule = proc_or_func_declaration_list + proc_or_func_declaration
                                  | proc_or_func_declaration
                                  | Empty;

            proc_or_func_declaration.Rule = MakePlusRule(proc_or_func_declaration, procedure_declaration)
                                   | MakePlusRule(proc_or_func_declaration, function_declaration)
                                   | Empty;

            procedure_declaration.Rule = PROCEDURE + identifier + LEFTPAREN + parametros + RIGHTPAREN + SEMICOLON + variable_definition_part + BEGIN + instrucciones + END + SEMICOLON
                                | PROCEDURE + identifier + LEFTPAREN + RIGHTPAREN + SEMICOLON + variable_definition_part + BEGIN + instrucciones + END + SEMICOLON;
            procedure_declaration.ErrorRule = SyntaxError + SEMICOLON;

            function_declaration.Rule = FUNCTION + identifier + LEFTPAREN + parametros + RIGHTPAREN + COLON + tipo_funcion + SEMICOLON + variable_definition_part + BEGIN + instrucciones + END + SEMICOLON //{:RESULT = new Function(a, b, c, d);:}
                          | FUNCTION + identifier + LEFTPAREN + RIGHTPAREN + COLON + tipo_funcion + SEMICOLON + variable_definition_part + BEGIN + instrucciones + END + SEMICOLON; //{:RESULT = new Function(a, b, c);:}
            function_declaration.ErrorRule = SyntaxError + SEMICOLON;

            parametros.Rule = MakePlusRule(parametros, parametro)
                     | Empty;

            parametro.Rule = identifier + COLON + tipo
                     | identifier + COLON + tipo + SEMICOLON
                     | VAR + identifier + COLON + tipo
                     | VAR + identifier + COLON + tipo + SEMICOLON;
            parametro.ErrorRule = SyntaxError + SEMICOLON
                    | SyntaxError + VAR
                    | SyntaxError + COLON;

            lista_identificadores.Rule = MakePlusRule(lista_identificadores, COMMA, identifier)
                                | Empty;
            lista_identificadores.ErrorRule = SyntaxError + COMMA;

            declaracion.Rule = identifier + COLON + tipo //{:RESULT = new Declaracion(b, a);:}
                      | declaracion_array; //{:RESULT = new DeclaracionArreglo(b, a, c);:}
            declaracion.ErrorRule = SyntaxError + COLON;

            declaracion_array.Rule = identifier + COLON + ARRAY + LEFTBRAC + ranges_list + RIGHTBRAC + OF + tipo;
            declaracion_array.ErrorRule = SyntaxError + COLON
                             | SyntaxError + ARRAY
                             | SyntaxError + LEFTBRAC
                             | SyntaxError + RIGHTBRAC
                             | SyntaxError + OF;

            ranges_list.Rule = MakePlusRule(ranges_list, COMMA, range);

            range.Rule = expresion + DOUBLEDOT + expresion;
            range.ErrorRule = SyntaxError + DOUBLEDOT; ;

            //dimensiones.Rule = dimensiones + LEFTBRAC + expresion + RIGHTBRAC //{:RESULT = a; RESULT.add(b);:}
            //         | LEFTBRAC + expresion + RIGHTBRAC; //{:RESULT = new LinkedList<>(); RESULT.add(a);:}
            //dimensiones.ErrorRule = SyntaxError + LEFTBRAC
            //           | SyntaxError + RIGHTBRAC;

            dimensiones.Rule = MakePlusRule(dimensiones, COMMA, expresion);

            expresiones.Rule = MakeListRule(expresiones, COMMA, expresion)
                      | Empty;

            instrucciones.Rule = MakePlusRule(instrucciones, instruccion)
                        | Empty;


            instruccion.Rule = WRITE + LEFTPAREN + expresiones + RIGHTPAREN + SEMICOLON  //{:RESULT = new Imprimir(a);:}
                     | WRITELN + LEFTPAREN + expresiones + RIGHTPAREN + SEMICOLON
                     | GRAPHTS + LEFTPAREN + RIGHTPAREN + SEMICOLON
                     //| VAR + declaracion  //{:RESULT = a;:}
                     | identifier + COLON + EQUAL + expresion + SEMICOLON //{:RESULT = new Asignacion(a, b);:}
                     | identifier + COLON + EQUAL + expresion
                     //| identifier + dimensiones + COLON + EQUAL + expresion + SEMICOLON //{:RESULT = new AsignacionArreglo(a, b, c);:}
                     | identifier + LEFTBRAC + dimensiones + RIGHTBRAC + COLON + EQUAL + expresion + SEMICOLON
                     | instruccion_if_sup  //{:RESULT = a;:}
                     | WHILE + expresion + DO + BEGIN + instrucciones + END + SEMICOLON //{:RESULT = new While(a, b);:}
                     | FOR + identifier + COLON + EQUAL + expresion + TO + expresion + DO + BEGIN + instrucciones + END + SEMICOLON
                     | FOR + identifier + COLON + EQUAL + expresion + DOWNTO + expresion + DO + BEGIN + instrucciones + END + SEMICOLON
                     | REPEAT + instrucciones + UNTIL + expresion + SEMICOLON
                     | case_statement
                     //| RFOR LEFTPAREN identifier:a EQUAL expresion: b SEMICOLON expresion: c SEMICOLON identifier: d EQUAL expresion: e RIGHTPAREN LLAVIZQ instrucciones:f LLAVDER{:RESULT = new For(new Asignacion(a, b), c, new Asignacion(d, e), f);:}
                     | functionOrProcedureCall               //{:RESULT = new LlamadaFuncion(a, new LinkedList<>());:}
                                                             //| RETURN + SEMICOLON             {:RESULT = new Return();:}
                     | EXIT + LEFTPAREN + expresion + RIGHTPAREN + SEMICOLON //{:RESULT = new Return(a);:}
                     | BREAK + SEMICOLON //{:RESULT = new Break();:}
                     | CONTINUE + SEMICOLON;
            instruccion.ErrorRule = SyntaxError + END
                                  | SyntaxError + SEMICOLON
                                  | SyntaxError + LEFTPAREN
                                  | SyntaxError + RIGHTPAREN
                                  | SyntaxError + SEMICOLON
                                  | SyntaxError + COLON
                                  | SyntaxError + EQUAL
                                  | SyntaxError + TO
                                  | SyntaxError + DO
                                  | SyntaxError + BEGIN;


            functionOrProcedureCall.Rule = identifier + LEFTPAREN + expresiones + RIGHTPAREN
                       | identifier + LEFTPAREN + expresiones + RIGHTPAREN + SEMICOLON
                       | identifier + LEFTPAREN + RIGHTPAREN
                       | identifier + LEFTPAREN + RIGHTPAREN + SEMICOLON;
            functionOrProcedureCall.ErrorRule = SyntaxError + LEFTPAREN
                                   | SyntaxError + RIGHTPAREN
                                   | SyntaxError + SEMICOLON;

            tipo.Rule = INTEGERtype  //{:RESULT = a;:}
                | REALType
                | STRINGType //:a  {:RESULT = a;:}
                | BOOLEANtype; //:a {:RESULT = a;:}


            tipo_funcion.Rule = tipo;//  {:RESULT = a;:}

            case_statement.Rule = CASE + expresion + OF + case_elements + END + SEMICOLON;
            case_statement.ErrorRule = SyntaxError + OF
                          | SyntaxError + END
                          | SyntaxError + SEMICOLON;

            case_elements.Rule = MakePlusRule(case_elements, case_element);

            case_element.Rule = expresion + COLON + BEGIN + instrucciones + END + SEMICOLON
                        | ELSE + BEGIN + instrucciones + END + SEMICOLON;

            expresion.Rule =
                   //MENOS       expresion: a         {:RESULT = new Operacion(a, Operacion.Tipo_operacion.NEGATIVO);:}% prec UMENOS
                   expresion + PLUS + expresion        //{:RESULT = new Operacion(a, b, Operacion.Tipo_operacion.SUMA);:}
                 | expresion + MINUS + expresion         //{:RESULT = new Operacion(a, b, Operacion.Tipo_operacion.RESTA);:}
                 | expresion + STAR + expresion         //{:RESULT = new Operacion(a, b, Operacion.Tipo_operacion.MULTIPLICACION);:}
                 | expresion + DIV + expresion         //{:RESULT = new Operacion(a, b, Operacion.Tipo_operacion.DIVISION);:}
                 | expresion + DIVKEYWORKD + expresion
                 | expresion + MOD + expresion
                 | expresion + GREATER + expresion         //{:RESULT = new Operacion(a, b, Operacion.Tipo_operacion.MAYOR_QUE);:}
                 | expresion + SMALLERTHAN + expresion         //{:RESULT = new Operacion(a, b, Operacion.Tipo_operacion.MENOR_QUE);:}
                 | expresion + SMALLEROREQUAL + expresion         //{:RESULT = new Operacion(a, b, Operacion.Tipo_operacion.MENOR_IGUAL_QUE);:}
                 | expresion + GREATHEROREQUAL + expresion         //{:RESULT = new Operacion(a, b, Operacion.Tipo_operacion.MAYOR_IGUAL_QUE);:}
                 | expresion + NOTEQUAL + expresion         //{:RESULT = new Operacion(a, b, Operacion.Tipo_operacion.DIFERENTE_QUE);:}
                 | expresion + EQUAL + expresion         //{:RESULT = new Operacion(a, b, Operacion.Tipo_operacion.IGUAL_QUE);:}
                 | NOT + expresion                     //{:RESULT = new Operacion(a, Operacion.Tipo_operacion.NOT);:}% prec RNOT
                 | expresion + OR + expresion          //{:RESULT = new Operacion(a, b, Operacion.Tipo_operacion.OR);:}
                 | expresion + AND + expresion         //{:RESULT = new Operacion(a, b, Operacion.Tipo_operacion.AND);:}
                                                       //| expresion + COMMA + expresion                                      //| expresion:a CONCAT      expresion: b         //{:RESULT = new Operacion(a, b, Operacion.Tipo_operacion.CONCATENACION);:}
                 | LEFTPAREN + expresion + RIGHTPAREN       //{:RESULT = a;:}
                 | INTEGER                                     //{:RESULT = new Operacion(new Double(a));:}
                 | REAL                                   //{:RESULT = new Operacion(new Double(a));:}
                 | STRING                                     //{:RESULT = new Operacion(a, Operacion.Tipo_operacion.CADENA);:}
                 | TRUE                                      //{:RESULT = new Operacion(a, Operacion.Tipo_operacion.TRUE);:}
                 | FALSE                                     //{:RESULT = new Operacion(a, Operacion.Tipo_operacion.FALSE);:}
                 | identifier                                 //{:RESULT = new Operacion(a, Operacion.Tipo_operacion.identifier);:}
                                                              //| identifier + LEFTPAREN + expresion + RIGHTPAREN //{:RESULT = new LlamadaFuncion(a, b);:}
                                                              //| identifier + LEFTPAREN + RIGHTPAREN               //{:RESULT = new LlamadaFuncion(a, new LinkedList<>());:}
                 | functionOrProcedureCall
                 | identifier + LEFTBRAC + dimensiones + RIGHTBRAC;                  //{:RESULT = new AccesoArreglo(a, b);:}



            instruccion_if_sup.Rule = instruccion_if + instruccion_else //{:RESULT = new If(a);:}
                                                                        //| instruccion_if + instrucciones_elseif  //{:RESULT = new If(a, b);:}
                                                                        //| instruccion_if + instrucciones_elseif + instruccion_else //{:RESULT = new If(a, b, c);:}
                             | instruccion_if
                             | Empty;  //{:RESULT = new If(a, b);:}


            instruccion_if.Rule = IF + LEFTPAREN + expresion + RIGHTPAREN + THEN + BEGIN + instrucciones + END; //{:RESULT = new SubIf(a, b);:}
            instruccion_if.ErrorRule = SyntaxError + LEFTPAREN
                          | SyntaxError + RIGHTPAREN
                          | SyntaxError + THEN
                          | SyntaxError + BEGIN
                          | SyntaxError + END;

            instrucciones_elseif.Rule = instrucciones_elseif + instruccion_elseif //:b{:RESULT = a; RESULT.add(b);:}
                               | instruccion_elseif; //:a{:RESULT = new LinkedList<>(); RESULT.add(a);:}


            instruccion_else.Rule = ELSE + BEGIN + instrucciones + END; //{:RESULT = new SubIf(a);:}
            instruccion_else.ErrorRule = SyntaxError + ELSE
                            | SyntaxError + BEGIN
                            | SyntaxError + END;
            #endregion

            #region Preferences
            this.Root = program;
            #endregion
        }
    }
}
