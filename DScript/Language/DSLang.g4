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

@lexer::header
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
		cmds=statements
		{
			$executable.CodeBlocks = $cmds.codeBlocks;
		}
	)?	EOF
	;

statements returns [List<ICodeBlock> codeBlocks]
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
	(
			vd=variable_def { $codeBlock = $vd.codeBlock; }
		|	vs=variable_set { $codeBlock = $vs.codeBlock; }
		|	cmd=command { $codeBlock = $cmd.codeBlock; }
		|	vi=variable_info { $codeBlock = $vi.codeBlock; }
		|	fd=function_def { $codeBlock = $fd.codeBlock; }
		|	md=module_def { $codeBlock = $md.codeBlock; }
		|	bs=break_stm { $codeBlock = $bs.codeBlock; }
		|	cs=continue_stm { $codeBlock = $cs.codeBlock; }
		|	es=export_stm { $codeBlock = $es.codeBlock; }
		|	gs=global_stm { $codeBlock = $gs.codeBlock; }
		|	pc=parent_ctx { $codeBlock = $pc.codeBlock; }
		|	gc=global_ctx { $codeBlock = $gc.codeBlock; }
		|	sc=self_ctx {$codeBlock = $sc.codeBlock; }
	)
	(
		'.' call=statement
		{
			List<IArgument> args = new List<IArgument>();
			args.Add(new CodeBlockArgument() { Code = $codeBlock });
			args.Add(new ConstantArgument(new GenericValue<ICodeBlock>($call.codeBlock)));
			ICodeBlock call = new CodeBlock()
				{
					Command = "call_context",
					Arguments = args
				};
			$codeBlock = call;
		}
	)?
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

function_def returns [ICodeBlock codeBlock]
	:	FUNCTION_DEF name=IDENT
	{
		List<IArgument> paramArgs = new List<IArgument>();
	}
	(
		GROUP_START
		(
			VAR_SPEC argName=IDENT
			{
				paramArgs.Add(new ConstantArgument(new GenericValue<string>($argName.text)));
			}
		)*
		GROUP_END
	)?
		exe=code_block
		{
			List<IArgument> args = new List<IArgument>();
			args.Add(new ConstantArgument(new GenericValue<string>($name.text)));
			List<IArgument> cbArgs = new List<IArgument>();
			cbArgs.Add(new ExecutableArgument()
				{
					Executable = $exe.executable
				});
			args.Add(new ConstantArgument(new GenericValue<IExecutable>($exe.executable)));
			args.AddRange(paramArgs);
			$codeBlock = new CodeBlock()
				{
					Command = "define_func",
					Arguments = args
				};
		}
	;

module_def returns [ICodeBlock codeBlock]
	:	MODULE_DEF VAR_SPEC module=IDENT
		{
			List<IArgument> args = new List<IArgument>();
			args.Add(new ConstantArgument(new GenericValue<string>($module.text)));

			$codeBlock = new CodeBlock()
				{
					Command = "define_module",
					Arguments = args
				};
		}
	;

break_stm returns [ICodeBlock codeBlock]
	:	BREAK_SCOPE
	{
		List<IArgument> rargs = new List<IArgument>();
		$codeBlock = new CodeBlock()
			{
				Command = "break_execution",
				Arguments = rargs
			};
	}
	(
		EXECUTE_START stm=statement EXECUTE_END
		{
			rargs.Add(new CodeBlockArgument() { Code = $stm.codeBlock });
		}
	)?
	;

continue_stm returns [ICodeBlock codeBlock]
	:	CONTINUE_SCOPE
	{
		$codeBlock = new CodeBlock()
			{
				Command = "cancel_execution",
				Arguments = new List<IArgument>()
			};
	}
	;

export_stm returns [ICodeBlock codeBlock]
	:	EXPORT_SPEC
	{
		ICodeBlock ctxBlock = new CodeBlock()
			{
				Command = "parent_context",
				Arguments = new List<IArgument>()
			};
	}
	(
		EXECUTE_START ctxStm=statement EXECUTE_END
		{
			ctxBlock = $ctxStm.codeBlock;
		}
	)?
		stm=statement
	{
		List<IArgument> args = new List<IArgument>();
		args.Add(new CodeBlockArgument()
			{
				Code = ctxBlock
			});
		args.Add(new ConstantArgument(new GenericValue<ICodeBlock>($stm.codeBlock)));
		$codeBlock = new CodeBlock()
			{
				Command = "export_context",
				Arguments = args
			};
	}
	;

global_stm returns [ICodeBlock codeBlock]
	:	GLOBAL_SPEC stm=statement
	{
		List<IArgument> args = new List<IArgument>();
		args.Add(new CodeBlockArgument()
			{
				Code = new CodeBlock()
					{
						Command = "global_context",
						Arguments = new List<IArgument>()
					}
			});
		args.Add(new CodeBlockArgument()
			{
				Code = $stm.codeBlock
			});
		$codeBlock = new CodeBlock()
			{
				Command = "export_context",
				Arguments = args
			};
	}
	;

parent_ctx returns [ICodeBlock codeBlock]
	:	PARENT_SPEC
	{
		$codeBlock = new CodeBlock()
			{
				Command = "parent_context",
				Arguments = new List<IArgument>()
			};
	}
	;

global_ctx returns [ICodeBlock codeBlock]
	:	GLOBAL_SPEC
	{
		$codeBlock = new CodeBlock()
			{
				Command = "global_context",
				Arguments = new List<IArgument>()
			};
	}
	;

self_ctx returns [ICodeBlock codeBlock]
	:	SELF_SPEC
	{
		$codeBlock = new CodeBlock()
			{
				Command = "self_context",
				Arguments = new List<IArgument>()
			};
	}
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
	|	eblock=execute_block
		{
			$result = new ExecutableArgument()
				{
					Executable = $eblock.executable
				};
		}
	|	cblock=code_block
		{
			List<IArgument> cbArgs = new List<IArgument>();
			cbArgs.Add(new ExecutableArgument()
				{
					Executable = $cblock.executable
				});
			$result = new ConstantArgument(new GenericValue<IExecutable>($cblock.executable));
		}
	|	exe=statement
		{
			$result = new CodeBlockArgument()
				{
					Code = $exe.codeBlock
				};
		}
	;

code_block returns [IExecutable executable]
	:
	{
		$executable = new Executable()
			{
				CodeBlocks = new List<ICodeBlock>()
			};
	}
		BLOCK_START
	(
		cmds=statements
		{
			$executable.CodeBlocks = $cmds.codeBlocks;
		}
	)? BLOCK_END
	;

execute_block returns [IExecutable executable]
	:
	{
		$executable = new Executable()
			{
				CodeBlocks = new List<ICodeBlock>()
			};
	}
		EXECUTE_START
	(
		cmds=statements
		{
			$executable.CodeBlocks = $cmds.codeBlocks;
		}
	)? EXECUTE_END
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

BLOCK_START
	:	'{'
	;

BLOCK_END
	:	'}'
	;

EXECUTE_START
	:	'['
	;

EXECUTE_END
	:	']'
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

FUNCTION_DEF
	:	'func'
	;

MODULE_DEF
	:	'module'
	;

EXPORT_SPEC
	:	'export'
	;

PARENT_SPEC
	:	'parent'
	;

GLOBAL_SPEC
	:	'global'
	;

SELF_SPEC
	:	'self'
	;

BREAK_SCOPE
	:	'break'
	;

CONTINUE_SCOPE
	:	'continue'
	;

EQUALS
	:	'='
	;

CONTEXT_ENTER
	:	'.'
	;

IDENT
	:	([a-zA-Z] | '_') ([0-9a-zA-Z] | '-' | '_')*
	;

WS
	:	[ \n\t\r] -> channel(HIDDEN)
	;

COMMENT
	:	'#' ~[\r\n]* -> channel(HIDDEN)
	;