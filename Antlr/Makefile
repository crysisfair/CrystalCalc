GRAM=Expr
ANTLR=java -jar .\\tool\\antlr-4.7-complete.jar
RUNTIME=Antlr4.Runtime.Standard.dll
OPT=-Dlanguage=CSharp -no-listener -visitor
COMPILER=csc.exe
TEST_FILE=.\\test\\test.txt

all:antlr compile
.PHONY:all

antlr:
	$(ANTLR) $(GRAM).g4 $(OPT)

compile:
	cp ./tool/$(RUNTIME) ./out
	$(COMPILER) /r:./tool/$(RUNTIME) /out:./out/$(GRAM).exe *.cs

run:
	.\\out\\$(GRAM).exe $(TEST_FILE)

.PHONY:clean
clean:
	rm -rf ./out/*.*
