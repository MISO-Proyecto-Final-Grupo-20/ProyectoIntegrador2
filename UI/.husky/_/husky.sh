# filepath: z:\MISO\ProyectoIntegrador2\UI\.husky\_/husky.sh
#!/usr/bin/env sh
if [ -z "$husky_skip_init" ]; then
  debug () {
    [ "$HUSKY_DEBUG" = "1" ] && echo "husky (debug) - $1"
  }

  readonly hook_name="$(basename -- "$0")"
  debug "starting $hook_name hook"

  if [ "$HUSKY" = "0" ]; then
    debug "HUSKY env variable is set to 0, skipping hook"
    exit 0
  fi

  if [ ! -f ~/.huskyrc ]; then
    debug "~/.huskyrc is missing, skipping hook"
    exit 0
  fi

  . ~/.huskyrc
  debug "~/.huskyrc sourced"
fi