/* Helper macros for scaling dialog objects to fonts. */
#define FONT_SCALE_X(size) ROUND(((size / 5.0f) * text_length (font, "X")))
#define FONT_SCALE_Y(size) ROUND(((size / 6.0f) * text_height (font)))

static FONT *old_font = NULL;

static INLINE void push_font (FONT *new_font)
{
   /* Saves the current font to be restored later by pop_font(). */

   if (!new_font)
      return;
   if (new_font == font)
      return;

   old_font = font;
   font = new_font;
}

static INLINE void pop_font (void)
{
   if (!old_font)
      return;

   font = old_font;
}

int sl_idle (int message, DIALOG *dialog, int key)
{
   /* A simple MSG_IDLE handler object that allows other objects to play
      nice with the GUI during blocking hard loops. */

   switch (message)
   {
      case MSG_IDLE:
      {
         gui_heartbeat ();

         break;
      }

      default:
         break;
   }

   return (D_O_K);
}

int sl_text (int message, DIALOG *dialog, int key)
{
   /*
      Standard text object with shadow and customizeable font.

      dp2 = Text to print.
      dp3 = Font.

      If the 'key' field of the DIALOG* is set, the next object in the
      dialog will be activated.  However, it is recommended that you tie the
      keyboard shortcut directly to that object instead, if possible.
   */

   RT_ASSERT(dialog);

   switch (message)
   {
      case MSG_DRAW:
      {
         BITMAP *bmp;
         int x, y;

         /* Calculate coordinates. */
         x = dialog->x;
         y = dialog->y;

         /* Select font. */
         push_font (dialog->dp3);

         /* Get drawing surface. */
         bmp = gui_get_screen ();

         /* Draw text shadow. */
         gui_textout_ex (bmp, dialog->dp2, (x + 1), (y + 1),
            GUI_SHADOW_COLOR, -1, FALSE);

         /* Draw text. */
         if (dialog->flags & D_DISABLED)
         {
            gui_textout_ex (bmp, dialog->dp2, x, y, GUI_DISABLED_COLOR, -1,
               FALSE);
         }
         else
         {
            gui_textout_ex (bmp, dialog->dp2, x, y, GUI_TEXT_COLOR, -1,
               FALSE);
         }

         /* Unselect font. */
         pop_font ();

         break;
      }

      case MSG_KEY:
      {
         int index = 0;

         while (&active_dialog[index] != dialog)
            index++;

         offer_focus (active_dialog, (index + 1), &index, TRUE);

         break;
      }

      default:
         break;
   }

   return (D_O_K);
}

int sl_ctext (int message, DIALOG *dialog, int key)
{
   /*
      Centered text object.

      Parameters - see sl_text().
   */

   RT_ASSERT(dialog);

   switch (message)
   {
      case MSG_DRAW:
      {
         BITMAP *bmp;
         int x, y;

         /* Calculate coordinates. */
         x = dialog->x; 
         y = dialog->y;

         x += ((dialog->w / 2) - (text_length (font, dialog->dp2) / 2));

         /* Select font. */
         push_font (dialog->dp3);

         /* Get drawing surface. */
         bmp = gui_get_screen ();

         /* Draw text shadow. */
         gui_textout_ex (bmp, dialog->dp2, (x + 1), (y + 1),
            GUI_SHADOW_COLOR, -1, FALSE);

         /* Draw text. */
         if (dialog->flags & D_DISABLED)
         {
            gui_textout_ex (bmp, dialog->dp2, x, y, GUI_DISABLED_COLOR, -1,
               FALSE);
         }
         else
         {
            gui_textout_ex (bmp, dialog->dp2, x, y, GUI_TEXT_COLOR, -1,
               FALSE);
         }

         /* Unselect font. */
         pop_font ();

         return (D_O_K);
      }

      default:
         break;
   }

   return (sl_text (message, dialog, key));
}

#define SL_FRAME_END    0xf0

int sl_frame (int message, DIALOG *dialog, int key)
{
   /*
      Moveable window object with titlebar.

      dp2 = Titlebar text.
      dp3 = Titlebar font.
   */

   int mx, my; /* Move. */

   RT_ASSERT(dialog);

   switch (message)
   {
      case MSG_DRAW:
      {
         BITMAP *bmp;
         int x1, y1, x2, y2;
         int tx, ty; /* Text. */

         /* Calculate box coordinates. */
         x1 = dialog->x;
         y1 = dialog->y;
         x2 = ((x1 + dialog->w) - 1);
         y2 = ((y1 + dialog->h) - 1);

         /* Calculate text coordinates. */
         tx = (x1 + 6);
         ty = (y1 + 6);

         /* Get drawing surface. */
         bmp = gui_get_screen ();

         /* Draw shadow. */
         rect (bmp, (x1 + 1), (y1 + 1), (x2 + 1), (y2 + 1),
            GUI_SHADOW_COLOR);

         /* Draw filling. */
         if (GUI_GRADIENT_START_COLOR == GUI_GRADIENT_END_COLOR)
         {
            /* Accelerated solid drawing. */
            rectfill (bmp, x1, y1, x2, y2, GUI_GRADIENT_START_COLOR);
         }
         else
         {
            /* Gradient fill. */

            int slice;

            video_create_gradient (GUI_GRADIENT_START_COLOR,
               GUI_GRADIENT_END_COLOR, dialog->w, 0, 0);

            for (slice = 0; slice < dialog->w; slice++)
            {
               int xo, yo;

               xo = (x1 + slice);

               for (yo = y1; yo <= y2; yo++)
               {
                  putpixel (bmp, xo, yo, video_create_gradient (0, 0, 0, xo,
                     yo));
                }
            }
         }

         /* Draw border. */
         rect (bmp, x1, y1, x2, y2, GUI_BORDER_COLOR);

         /* Select font. */
         push_font (dialog->dp3);

         /* Draw titlebar filling. */
         rectfill (bmp, (x1 + 1), (y1 + 1), (x2 - 1), ((text_height (font) +
            ty) + 4), GUI_FILL_COLOR);

         /* Draw text shadow. */
         textout_ex (bmp, font, dialog->dp2, (tx + 1), (ty + 1),
            GUI_SHADOW_COLOR, -1);

         /* Draw text. */
         textout_ex (bmp, font, dialog->dp2, tx, ty, GUI_TEXT_COLOR, -1);

         /* Draw separator. */
         hline (bmp, (x1 + 1), ((text_height (font) + ty) + 4), (x2 - 1),
            GUI_BORDER_COLOR);

         /* Draw separator shadow. */
         hline (bmp, ((x1 + 1) - 1), (((text_height (font) + ty) + 4) + 1),
            ((x2 - 1) + 1), GUI_SHADOW_COLOR);

         /* TODO: Find out what this is for. */
         hline (bmp, ((x1 + 1) - 1), (((text_height (font) + ty) + 4) + 2),
            ((x2 - 1) + 1), GUI_BORDER_COLOR);

         /* Unselect font. */
         pop_font ();

         break;
      }

      case MSG_CLICK:
      {
         int x1, y1, x2, y2;
         int tbx1, tby1, tbx2, tby2;
         BOOL box_was_drawn = FALSE;
         int ox, oy; /* old */
         BITMAP *bmp;

         if (dialog->flags & D_DISABLED)
         {
            /* Disabled frames can't be dragged. */
            return (D_O_K);
         }

         /* Calculate box coordinates. */
         x1 = dialog->x;
         y1 = dialog->y;
         x2 = ((x1 + dialog->w) - 1);
         y2 = ((y1 + dialog->h) - 1);

         /* Calculate titlebar area. */
         tbx1 = (x1 + 1);
         tby1 = (y1 + 1);
         tbx2 = (x2 - 1);
         tby2 = ((y1 + (text_height (font) + 10)) - 1);

         if (((mouse_x >= tbx1) && (mouse_x <= tbx2)) &&
             ((mouse_y >= tby1) && (mouse_y <= tby2)))
         {
            /* This is a drag operation. */

            mx = ox = mouse_x;
            my = oy = mouse_y;
   
            bmp = gui_get_screen ();
   
            while (mouse_b & 1)
            {
               int cx, cy; /* current */
   
               cx = mouse_x;
               cy = mouse_y;
   
               xor_mode (TRUE);
         
               if ((mx != cx) || (my != cy))
               {
                  scare_mouse ();
   
                  if (box_was_drawn)
                  {
                     /* TODO: This needs cleaned up. */
                     rect (bmp, (dialog->x + mx - ox), (dialog->y + my - oy),
                        (dialog->x + mx - ox + dialog->w - 1), (dialog->y + my
                           - oy + dialog->h - 1), GUI_BORDER_COLOR);
                  }
   
                  mx = cx;
                  my = cy;
   
                  /* TODO: This needs cleaned up. */
                  rect (bmp, (dialog->x + mx - ox), (dialog->y + my - oy),
                     (dialog->x + mx - ox + dialog->w - 1), (dialog->y + my -
                        oy + dialog->h - 1), GUI_BORDER_COLOR);
   
                  box_was_drawn = TRUE;
   
                  unscare_mouse ();
               }
   
               broadcast_dialog_message (MSG_IDLE, 0);
            }
   
            solid_mode ();
   
            dialog_x = dialog->x + mx - ox;
            dialog_y = dialog->y + my - oy;
      
            restart_dialog = TRUE;
      
            return (D_CLOSE);
         }

         break;
      }

      default:
         break;
   }

   return (D_O_K);
}

int sl_button (int message, DIALOG *dialog, int key)
{
   /*
      Pushbutton object with a callback.

      dp2 = Callback function.
   */

   int (*handler) (DIALOG *);
   int result;

   RT_ASSERT(dialog);

   handler = dialog->dp2;

   switch (message)
   {
      case MSG_CLICK:
      case MSG_KEY:
      {
         if (!handler)
            break;

         dialog->flags |= D_SELECTED;

         scare_mouse ();
         object_message (dialog, MSG_DRAW, 0);
         unscare_mouse ();

         result = handler (dialog);

         dialog->flags &= ~D_SELECTED;

         scare_mouse ();
         object_message (dialog, MSG_DRAW, 0);
         unscare_mouse ();

         return (result);
      }

      default:
         break;
   }

   return (d_button_proc (message, dialog, key));
}

int sl_checkbox (int message, DIALOG *dialog, int key)
{
   /*
      Checkbox object with a callback.

      dp2 = Callback function.
   */

   RT_ASSERT(dialog);

   switch (message)
   {
      case MSG_CLICK:
      case MSG_KEY:
      {
         int (*handler) (DIALOG *);

         dialog->flags ^= D_SELECTED;

         scare_mouse ();
         object_message (dialog, MSG_DRAW, 0);
         unscare_mouse ();

         handler = dialog->dp2;
         if (handler)
            return (handler (dialog));

         return (D_O_K);
      }

      case MSG_DRAW:
      {
         int x1, y1, x2, y2;
         int c;
         BITMAP *bmp;

         x1 = dialog->x;
         y1 = dialog->y;
         x2 = ((dialog->x + dialog->h) - 1);
         y2 = ((dialog->y + dialog->h) - 1);

         if (dialog->flags & D_DISABLED)
            c = GUI_DISABLED_COLOR;
         else
            c = GUI_TEXT_COLOR;

         bmp = gui_get_screen ();

         /* Draw box border shadow. */
         rect (bmp, (x1 + 1), (y1 + 1), (x2 + 1), (y2 + 1),
            GUI_SHADOW_COLOR);

         /* Draw inside of box. */
         rectfill (bmp, x1, y1, x2, y2, GUI_FILL_COLOR);

         /* Draw box border. */
         rect (bmp, x1, y1, x2, y2, c);

         if (dialog->flags & D_SELECTED)
         {
            /* Draw check mark. */
            line (bmp, x1, y1, x2, y2, c);
            line (bmp, x2, y1, x1, y2, c);
         }

         if (dialog->flags & D_GOTFOCUS)
         {
            /* Draw selection focus. */
            rect (bmp, (x1 + 2), (y1 + 2), (x2 - 2), (y2 - 2),
               GUI_BORDER_COLOR);
         }

         y1++;
         x2 += 4;

         /* Draw text shadow. */
         gui_textout_ex (bmp, dialog->dp, (x2 + 1), (y1 + 1),
            GUI_SHADOW_COLOR, -1, FALSE);

         /* Draw text. */
         gui_textout_ex (bmp, dialog->dp, x2, y1, c, -1, FALSE);

         return (D_O_K);
      }

      default:
         break;
   }

   return (d_check_proc (message, dialog, key));
}

int sl_hr (int message, DIALOG *dialog, int key)
{
   /*
      Horizontal splitter.
   */

   RT_ASSERT(dialog);

   switch (message)
   {
      case MSG_DRAW:
      {
         int x, y, x2;
         BITMAP *bmp;

         x = dialog->x;
         y = dialog->y;
         x2 = ((x + dialog->w) - 1);

         bmp = gui_get_screen ();

         hline (bmp, x, y,       x2, GUI_LIGHT_SHADOW_COLOR);
         hline (bmp, x, (y + 1), x2, GUI_BACKGROUND_COLOR);
         hline (bmp, x, (y + 2), x2, GUI_BORDER_COLOR);

         break;
      }

      default:
         break;
   }

   return (D_O_K);
}

int sl_listbox (int message, DIALOG *dialog, int key)
{
   /*
      Listbox object with a callback.

      dp2 = Double-click callback function.
      dp3 = Single-click callback function.

      Keypresses are sent to dp2 if it is defined, otherwise to dp3.

      Since dp2 is used, multiple selections are not supported.

      Callback return codes (e.g D_CLOSE) are not supported.
   */

   int (*handler) (DIALOG *);
   void *saved_dp2;
   int result;

   RT_ASSERT(dialog);

   switch (message)
   {
      case MSG_CLICK:
      {
         handler = dialog->dp3;

         if (handler)
            handler (dialog);

         break;
      }

      case MSG_DCLICK:
      {
         handler = dialog->dp2;

         if (handler)
            handler (dialog);

         break;
      }     

      case MSG_KEY:
      {
         if (dialog->dp2)
            handler = dialog->dp2;
         else
            handler = dialog->dp3;

         if (handler)
            handler (dialog);

         break;
      }

      default:
         break;
   }

   saved_dp2 = dialog->dp2;
   dialog->dp2 = NULL;

   result = d_list_proc (message, dialog, key);

   dialog->dp2 = saved_dp2;

   return (result);
}

int sl_viewer (int message, DIALOG *dialog, int key)
{
   /*
      Read-only textbox object.
   */

   RT_ASSERT(dialog);

   switch (message)
   {
      case MSG_CHAR:
      {
         switch ((key >> 8))
         {
            case KEY_UP:
            case KEY_DOWN:
            case KEY_LEFT:
            case KEY_RIGHT:
            case KEY_HOME:
            case KEY_END:
            case KEY_PGUP:
            case KEY_PGDN:
               return (d_textbox_proc (message, dialog, key));

            default:
               break;
         }

         break;
      }

      default:
         return (d_textbox_proc (message, dialog, key));
   }

   return (D_O_K);
}

int sl_editbox (int message, DIALOG *dialog, int key)
{
   /*
      Edit box with shadow.

      Parameters are the same as d_edit_proc.
   */

   RT_ASSERT(dialog);

   switch (message)
   {
      case MSG_DRAW:
      {
         int saved_x, saved_y, saved_w, saved_h;

         /* Draw shadow box. */
         d_shadow_box_proc (message, dialog, key);

         saved_x = dialog->x;
         saved_y = dialog->y;
         saved_w = dialog->w;
         saved_h = dialog->h;

         /* Place edit box inside of shadow box. */

         dialog->x += FONT_SCALE_X(2);
         dialog->y += FONT_SCALE_Y(4);
         dialog->w -= FONT_SCALE_X(4);
         dialog->h -= FONT_SCALE_Y(2);

         d_edit_proc (message, dialog, key);

         dialog->x = saved_x;
         dialog->y = saved_y;
         dialog->w = saved_w;
         dialog->h = saved_h;

         break;
      }

      default:
         return (d_edit_proc (message, dialog, key));
   }

   return (D_O_K);
}

int sl_editbox2 (int message, DIALOG *dialog, int key)
{
   /*
      Edit box that invokes a callback when ENTER is pressed.

      dp2 - Callback;
   */

   RT_ASSERT(dialog);

   switch (message)
   {
      case MSG_CHAR:
      {
         switch ((key >> 8))
         {
            case KEY_ENTER:
            case KEY_ENTER_PAD:
            {
               int (*handler) (DIALOG *);

               handler = dialog->dp2;

               if (handler)
                  return (handler (dialog));

               break;
            }

            default:
               break;
         }

         break;
      }

      default:
         break;
   }

   return (sl_editbox (message, dialog, key));
}

int sl_lobby_msgbox (int message, DIALOG *dialog, int key)
{
   /*
      Message input box for the NetPlay lobby dialog.  Sends a message over
      the network (in the form of a UTF-8 coded chat packet) after the ENTER
      key is pressed while the object is active.

      Note that the 'dp' field MUST point to a USTRING, otherwise the call
      to USTRING_CLEAR() could cause the program to crash.
   */

   RT_ASSERT(dialog);

   switch (message)
   {
      case MSG_CHAR:
      {
         switch ((key >> 8))
         {
            case KEY_ENTER:
            case KEY_ENTER_PAD:
            {
               /* Send message. */
               netplay_send_message (dialog->dp);

               /* Clear buffer. */
               USTRING_CLEAR(dialog->dp);

               /* Redraw object. */
               return (D_REDRAW);
            }

            default:
               return (d_edit_proc (message, dialog, key));
         }

         break;
      }

      default:
         return (d_edit_proc (message, dialog, key));
   }

   return (D_O_K);
}

int sl_radiobox (int message, DIALOG *dialog, int key)
{
   /*
      Radiobutton object with a callback.

      dp2 = Callback function.
   */

   RT_ASSERT(dialog);

   switch (message)
   {
      case MSG_CLICK:
      case MSG_KEY:
      {
         int (*handler) (DIALOG *);
         int value;

         handler = dialog->dp2;

         d_radio_proc (message, dialog, key);

         if (handler)
            return (handler (dialog));

         break;
      }

      case MSG_DRAW:
      {
         int x1, y1, x2, y2;
         int c;
         BITMAP *bmp;

         x1 = dialog->x;
         y1 = dialog->y;
         x2 = ((dialog->x + dialog->h) - 1);
         y2 = ((dialog->y + dialog->h) - 1);

         if (dialog->flags & D_DISABLED)
            c = GUI_DISABLED_COLOR;
         else
            c = GUI_TEXT_COLOR;

         bmp = gui_get_screen ();

         /* Draw box border shadow. */
         rect (bmp, (x1 + 1), (y1 + 1), (x2 + 1), (y2 + 1),
            GUI_SHADOW_COLOR);

         /* Draw inside of box. */
         rectfill (bmp, x1, y1, x2, y2, GUI_FILL_COLOR);

         /* Draw box border. */
         rect (bmp, x1, y1, x2, y2, c);

         if (dialog->flags & D_SELECTED)
         {
            /* Draw bullet. */
            rectfill (bmp, (x1 + 2), (y1 + 2), (x2 - 2), (y2 - 2), c);
         }

         if (dialog->flags & D_GOTFOCUS)
         {
            /* Draw selection focus. */
            rect (bmp, (x1 + 2), (y1 + 2), (x2 - 2), (y2 - 2),
               GUI_BORDER_COLOR);
         }

         y1++;
         x2 += 4;

         /* Draw text shadow. */
         gui_textout_ex (bmp, dialog->dp, (x2 + 1), (y1 + 1),
            GUI_SHADOW_COLOR, -1, FALSE);

         /* Draw text. */
         gui_textout_ex (bmp, dialog->dp, x2, y1, c, -1, FALSE);

         return (D_O_K);
      }

      default:
         break;
   }

   return (d_radio_proc (message, dialog, key));
}

int sl_x_button (int message, DIALOG *dialog, int key)
{
   /*
      Close button object for inclusion in an sl_frame object.

      To make it work, give it the D_EXIT flag.
   */

   int result;

   /* Select font. */
   push_font (small_font);

   result = d_button_proc (message, dialog, key);

   /* Unselect font. */
   pop_font ();

   return (result);
}

static void sl_draw_menu (int x, int y, int width, int height)
{
   BITMAP *bmp;

   /* Bug fix (Allegro 4.1.1+). */
   width--;
   height--;

   bmp = gui_get_screen ();

   vline (bmp, (x + width), (y + 1), (y + height), GUI_SHADOW_COLOR);
   hline (bmp, (x + 1), (y + height), (x + width), GUI_SHADOW_COLOR);

   rect (bmp, x, y, (x + (width - 1)), (y + (height - 1)),
      GUI_BORDER_COLOR);
}

static void sl_draw_menu_item (MENU *menu, int x, int y, int width, int
   height, int bar, int selected)
{
   int i, j;
   char buf[256], *tok;
   int old_fg = 0;
   BITMAP *bmp;

   RT_ASSERT(menu);

   /* TODO: Make sure all of this is fully Unicode compliant. */
   /* TODO: Clean all of this up. */

   bmp = gui_get_screen ();

   if (ugetc (menu->text))
   {
      i = 0;
      j = ugetc (menu->text);

      while ((j) && (j != '\t'))
      {
         i += usetc ((buf + i), j);
         j = ugetc ((menu->text + i));
      }

      usetc ((buf + i), 0);

      if (!bar && !selected)
      {
         int slice;

         if (GUI_GRADIENT_START_COLOR == GUI_GRADIENT_END_COLOR)
         {
            rectfill (bmp, x, y, (x + (width - 1)), (y + (text_height (font)
               + 3)), GUI_GRADIENT_START_COLOR);
         }
         else
         {
            video_create_gradient (GUI_GRADIENT_START_COLOR,
               GUI_GRADIENT_END_COLOR, width, 0, 0);
    
            for (slice = 0; slice < width; slice++)
            {
               int xo, yo;
    
               xo = (x + slice);
    
               for (yo = y; yo <= (y + (text_height (font) + 3)); yo++)
               {
                  putpixel (bmp, xo, yo, video_create_gradient (0, 0, 0, xo,
                     yo));
               }
            }
         }
      }
      else if (selected)
      {
         rectfill (bmp, x, y, (x + (width - 1)), (y + (text_height (font) +
            3)), GUI_SELECTED_COLOR);
      }
      else
      {
         rectfill (bmp, x, y, (x + (width - 1)), (y + (text_height (font) +
            3)), GUI_MENU_BAR_COLOR);
      }

      gui_textout_ex (bmp, buf, (x + 9), (y + 2), GUI_SHADOW_COLOR, -1,
         FALSE);

      if (menu->flags & D_DISABLED)
      {
         gui_textout_ex (bmp, buf, (x + 8), (y + 1), GUI_DISABLED_COLOR, -1,
            FALSE);
      }
      else
      {
         gui_textout_ex (bmp, buf, (x + 8), (y + 1), GUI_TEXT_COLOR, -1,
            FALSE);
      }

      if (j == '\t')
      {
         tok = ((menu->text + i) + uwidth ((menu->text + i)));

         gui_textout_ex (bmp, tok, (x + ((width - (gui_strlen (tok) - 10)) +
            1)), (y + 2), GUI_SHADOW_COLOR, -1, FALSE);

         if (menu -> flags & D_DISABLED)
         {
            gui_textout_ex (bmp, tok, (x + (width - (gui_strlen (tok) -
               10))), (y + 1), GUI_DISABLED_COLOR, -1, FALSE);
         }
         else
         {
            gui_textout_ex (bmp, tok, (x + (width - (gui_strlen (tok) -
               10))), (y + 1), GUI_TEXT_COLOR, -1, FALSE);
         }
      }

      if (menu->child && !bar)
      {
         int cy;

         cy = (y + (text_height (font) / 2));

         triangle (bmp, ((x + (width - 4)) + 1), (cy + 1), ((x + (width -
            8)) + 1), ((cy - 4) + 1), ((x + (width - 8)) + 1), ((cy + 4) +
               1), GUI_SHADOW_COLOR);
                                             
         if (menu->flags & D_DISABLED)
         {
            triangle (bmp, (x + (width - 4)), cy, (x + (width - 8)), (cy -
               4), (x + (width - 8)), (cy + 4), GUI_DISABLED_COLOR);
         }
         else
         {
            triangle (bmp, (x + (width - 4)), cy, (x + (width - 8)), (cy -
               4), (x + (width - 8)), (cy + 4), GUI_TEXT_COLOR);
         }
      }
   }
   else
   {
      int slice;

      if (GUI_GRADIENT_START_COLOR == GUI_GRADIENT_END_COLOR)
      {
         rectfill (bmp, x, y, (x + (width - 1)), (y + (text_height (font) +
            3)), GUI_GRADIENT_START_COLOR);
      }
      else
      {
         video_create_gradient (GUI_GRADIENT_START_COLOR,
            GUI_GRADIENT_END_COLOR, width, 0, 0);
    
         for (slice = 0; slice < width; slice++)
         {
            int xo, yo;
    
            xo = (x + slice);
    
            for (yo = y; yo <= (y + (text_height (font) + 3)); yo++)
            {
               putpixel (bmp, xo, yo, video_create_gradient (0, 0, 0, xo,
                  yo));
            }
         }
      }

      hline (bmp, x, (y + (text_height (font) / 2)), (x + width),
         GUI_LIGHT_SHADOW_COLOR);
      hline (bmp, x, (y + ((text_height (font) / 2) + 2)), (x + (width -
         1)), GUI_BORDER_COLOR);
      hline (bmp, (x - 1), (y + ((text_height (font) / 2) + 1)), (x +
         width), GUI_SHADOW_COLOR);
   }

   if (menu->flags & D_SELECTED)
   {
      circlefill (bmp, ((x + 3) + 1), ((y + (text_height (font) / 2)) + 1),
         2, GUI_SHADOW_COLOR);

      if (menu->flags & D_DISABLED)
      {
         circlefill (bmp, (x + 3), (y + (text_height (font) / 2)), 2,
            GUI_DISABLED_COLOR);
      }
      else
      {
         circlefill (bmp, (x + 3), (y + (text_height (font) / 2)), 2,
            GUI_TEXT_COLOR);
      }
   }
}
