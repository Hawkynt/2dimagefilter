# Makefile to build cbuild

CFLAGS=-O2 -W -Wall

ifneq ($(MINGDIR)$(DJDIR),)
	CC = gcc
	EXEEXT = .exe
else
	EXEEXT =
endif

CBUILD=cbuild$(EXEEXT)

all: $(CBUILD) exec

.PHONY: exec

$(CBUILD): cbuild.c
	$(CC) $(CFLAGS) -o $@ $<

exec: $(CBUILD)
	./$(CBUILD)
