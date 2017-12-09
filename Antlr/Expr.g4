grammar Expr;

prog    : stat+;

stat    : expr end=(';' | NEWLINE )         # printExpr
        | ID '=' expr end=(';' | NEWLINE )  # assign
        | NEWLINE                           # blank
        ;

expr    : expr op=('*'|'/') expr    # MulDiv
        | expr op=('+'|'-') expr    # AddSub
        | INT                       # int
        | ID                        # id
        | '(' expr ')' # parens
        ;

MUL     : '*' ; // assigns token name to '*' used above in grammar
DIV     : '/' ;
ADD     : '+' ;
SUB     : '-' ;
ID      : [a-z_A-Z]+;
INT     : [0-9]+;
NEWLINE :    '\r'? '\n';
WS      : [ \t] -> skip;
