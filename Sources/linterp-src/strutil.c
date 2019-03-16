/**
 * @file strutil.c
 * @brief String utility functions
 * @author Pascal Getreuer <getreuer@gmail.com>
 *
 *
 * Copyright (c) 2011, Pascal Getreuer
 * All rights reserved.
 *
 * This program is free software: you can use, modify and/or
 * redistribute it under the terms of the simplified BSD License. You
 * should have received a copy of this license along this program. If
 * not, see <http://www.opensource.org/licenses/bsd-license.html>.
 */

#include <ctype.h>
#include <math.h>
#include "strutil.h"

/**
 * @brief Eat whitespace characters in a string
 *
 * @param StrPtr char** pointing to a pointer to the character array
 * @return 1 if a non-space character is found, 0 if the string terminates
 *
 * This function advances a pointer to a string to consume as much whitespace
 * as possible.  Upon returning, *StrPos points to the first non-space
 * character.  If the string terminates without a non-space character, *StrPos
 * is the location of the null terminator.
 */
int EatWhitespace(const char **StrPtr)
{
    const char *Str = *StrPtr;

    while(isspace(*Str))
        Str++;

    *StrPtr = Str;
    return (*Str != '\0');
}


/**
 * @brief Read a number from a string
 * @param Number is a pointer to where to store the result
 * @param StrPtr char** pointing to a pointer to the character array
 * @param FloatAllowed whether decimal or scientific notations are allowed
 * @return 1 on success, 0 on failure
 *
 * The routine consumes leading whitespace and reads as many characters as
 * possible to form a valid floating-point number in decimal or scientific
 * notation.  Upon returning, *StrPos points to the character immediately
 * following the number.
 */
int ParseNumber(double *Number, const char **StrPtr, int FloatAllowed)
{
    const char *Str = *StrPtr;
    double Accum = 0, Div = 1, Exponent = 0;
    int Sign = 1, ExponentSign = 1;
    char c;


    /* Eat leading whitespace */
    if(!EatWhitespace(&Str))
        return 0;

    if(*Str == '-')        /* Read sign */
    {
        Sign = -1;
        Str++;
    }
    else if(*Str == '+')
        Str++;

    /* Read one or more digits appearing left of the decimal point */
    if(isdigit(c = *Str))
        Accum = c - '0';
    else
        return 0;               /* First character is not a digit */

    while(isdigit(c = *(++Str)))
        Accum = 10*Accum + (c - '0');

    if(c == '.')                /* There is a decimal point */
    {
        if(!FloatAllowed)
            return 0;

        /* Read zero or more digits appearing right of the decimal point */
        while(isdigit(c = *(++Str)))
        {
            Div *= 10;
            Accum += (c - '0')/Div;
        }
    }

    if(c == 'e' || c == 'E')    /* There is an exponent */
    {
        if(!FloatAllowed)
            return 0;

        Str++;

        if(*Str == '-')      /* Read exponent sign */
        {
            ExponentSign = -1;
            Str++;
        }
        else if(*Str == '+')
            Str++;

        /* Read digits in the exponent */
        if(isdigit(c = *Str))
        {
            Exponent = c - '0';

            while(isdigit(c = *(++Str)))
                Exponent = 10*Exponent + (c - '0');

            Exponent *= ExponentSign;
            Accum = Accum * pow(10, Exponent);
        }
        else
            return 0;
    }

    *Number = Sign*Accum;
    *StrPtr = Str;
    return 1;
}
