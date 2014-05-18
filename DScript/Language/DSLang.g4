grammar DSLang;

@parser::header
{
	#pragma warning disable 3021

	using DScript.Context;
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

compileUnit returns [IExecutable executable]
	:
	{
		$executable = new Executable()
			{
				CodeBlocks = new List<ICodeBlock>()
			};
	}
	(
		cmds=commands
		{
			$executable.CodeBlocks = $cmds.codeBlocks;
		}
	)?	EOF
	;

commands returns [List<ICodeBlock> codeBlocks]
	:
	{
		$codeBlocks = new List<ICodeBlock>();
	}
	(
		cmd=statement
		{
			$codeBlocks.Add($cmd.codeBlock);
		}
	)+
	;

statement returns [ICodeBlock codeBlock]
	:
		vd=variable_def { $codeBlock = $vd.codeBlock; }
	|	vs=variable_set { $codeBlock = $vs.codeBlock; }
	|	cmd=command { $codeBlock = $cmd.codeBlock; }
	|	vi=variable_info { $codeBlock = $vi.codeBlock; }
	;

variable_def returns [ICodeBlock codeBlock]
	:	VAR_DEF VAR_SPEC var=IDENT
	{
		$codeBlock = new CodeBlock()
			{
				Command = "define",
				Arguments = new List<IArgument>()
			};
		$codeBlock.Arguments.Add(new ConstantArgument(new GenericValue<string>($var.text)));
	}
	(
		EQUALS arg=argument
		{
			$codeBlock.Arguments.Add($arg.result);
		}
	)?
	;

variable_set returns [ICodeBlock codeBlock]
	:	VAR_SPEC var=IDENT EQUALS arg=argument
	{
		$codeBlock = new CodeBlock()
			{
				Command = "set",
				Arguments = new List<IArgument>()
			};
		$codeBlock.Arguments.Add(new ConstantArgument(new GenericValue<string>($var.text)));
		$codeBlock.Arguments.Add($arg.result);
	}
	;

variable_info returns [ICodeBlock codeBlock]
	:	VAR_SPEC var=IDENT
	{
		List<IArgument> vargs = new List<IArgument>();
		vargs.Add(new ConstantArgument(new GenericValue<string>($var.text)));
		$codeBlock = new CodeBlock()
		{
			Command = "get",
			Arguments = vargs
		};
	}
	(
		varArgs=arguments
		{
			vargs.AddRange($varArgs.args);
			$codeBlock.Command = vargs.Count == 1 ? "get" : "set";
		}
	)?
	;

command returns [ICodeBlock codeBlock]
	:	cmdName=IDENT args=arguments
	{
		$codeBlock = new CodeBlock()
			{
				Command = $cmdName.text,
				Arguments = $args.args
			};
	}
	;

arguments returns [List<IArgument> args]
	:
	{
		$args = new List<IArgument>();
	}
		GROUP_START
	(
		a1=argument
		{
			$args.Add($a1.result);
		}

		(
			ARGUMENT_SEPARATOR a2=argument
			{
				$args.Add($a2.result);
			}
		)*
	)? GROUP_END
	;

argument returns [IArgument result]
	:	str=STRING
		{
			$result = new ConstantArgument(new GenericValue<string>($str.text));
		}
	|	strExt=STRING_EXT
		{
			$result = new ConstantArgument(new GenericValue<string>($strExt.text));
		}
	|	num=NUMBER
		{
			$result = new ConstantArgument(new GenericValue<double>(double.Parse($num.text)));
		}
	|	VAR_SPEC var=IDENT
		{
			$result = new VariableArgument()
				{
					Variable = $var.text
				};
		}
	|	exe=statement
		{
			$result = new ExecutableArgument()
				{
					Code = $exe.codeBlock
				};
		}
	;

/*
 * Lexer Rules
 */

fragment ESCAPE_SEQUENCE
	:	'\\'
	(
		'\\'
	|	'"'
	|	'\''
	)
	;

STRING
	:
	(
		'"' ( ESCAPE_SEQUENCE | . )*? '"'
	|	'\'' ( ESCAPE_SEQUENCE | . )*? '\''
	)
	{
		Text = Text.Substring(1, Text.Length - 2)
				.Replace("\\\\", "\\")
				.Replace("\\\"", "\"")
				.Replace("\\\'", "\'");
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

VAR_DEF
	:	'var'
	;

EQUALS
	:	'='
	;

IDENT
	:	([a-zA-Z] | '_') ([0-9a-zA-Z] | '.' | '-' | '_')*
	;

WS
	:	[ \n\t\r] -> channel(HIDDEN)
	;

COMMENT
	:	'#' ~[\r\n]* -> channel(HIDDEN)
	;