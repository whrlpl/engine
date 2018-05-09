# Bytecode

## Instructions
(Where n is the current point on the stack)

- `NLIT` - Define a number literal; inserts it into the stack.
- `ILIT` - Define an integer literal
- `SLIT` - Define a string literal
- `OUT` - Output the last variable on the stack (expects string)
- `IN` - Inputs a string to the stack
- `LD` - Copy one location from the stack to the latest point
- `JMP` - Jump to a location in a file
- `ADD` - Add n and n-1 together
- `MULT` - Multiply n and n-1 together
- `DIV` - Divide n and n-1
- `SUB` - Subtract n from n-1
- `GSS` - Get the current stack size
- `PSH` - Push a variable to the stack
- `POP` - Get a variable from the stack

#Language
## Keywords:
- `pub`
- `priv`
- `prot`
- `func`
- `ASM`
- `for`
- `let`
- `ret`
- `dep`

##Types:
- `void`

##Separators:
- `;`
- `{`
- `}`
- `(`
- `)`
- `,`

##Operators:
- `<`
- `>`
- `-=`
- `+=`
- `==`
- `!=`
- `=`
- `+`
- `-`
- `*`
- `/`

##Literals:
- `"..."` - String
- `0` - Integer