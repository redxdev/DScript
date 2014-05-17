grammar DSLang;

@parser::header
{
	#pragma warning disable 3021

	using DScript.Context.Arguments;
}

@parser::members
{
	protected const int EOF = Eof;
}

@lexer::leader
{
	#pragma warning disable 3021
}

@lexer::members
{
	protected const int EOF = Eof;
	protected const int HIDDEN = Hidden;
}

/*
 * Parser Rules
 */

compileUnit
	:	commands? EOF
	;

commands
	:	command+
	;

command
	:	IDENT arguments
	;

arguments
	:	GROUP_START ( argument ( ARGUMENT_SEPARATOR argument )* )? GROUP_END
	;

argument
	:	STRING
	|	STRING_EXT
	|	NUMBER
	|	VAR_SPEC IDENT
	|	command
	;

/*
 * Lexer Rules
 */
fragment ESCAPE_SEQUENCE
	:	'\\'
	(
		'\\'
	|	'"'
	)
	;

STRING
	:	'"' ( ESCAPE_SEQUENCE | . )*? '"'
	{
		Text = Text.Substring(1, Text.Length - 2)
				.Replace("\\\\", "\\")
				.Replace("\\\"", "\"");
	}
	;

STRING_EXT
	:	'[[' .*? ']]'
	{
		Text = Text.Substring(2, Text.Length - 4);
	}
	;

NUMBER
	:	'-'?
	(
		[0-9]* '.' [0-9]+
	|	[0-9]+
	)
	;

ARGUMENT_SEPARATOR
	:	','
	;

GROUP_START
	:	'('
	;

GROUP_END
	:	')'
	;

VAR_SPEC
	:	'$'
	;

IDENT
	:	[a-zA-Z_] [0-9a-zA-Z.-_]*
	;

WS
	:	[ \n\t\r] -> channel(HIDDEN)
	;

COMMENT
	:	'#' ~[\r\n]* -> channel(HIDDEN)
	;