# VariabledComparer

A simple app that allows you to compare text with more templates and shows you to which template the text conforms.

Template can contain literal text and variables in the following format: `#name_of_the_variable_nr_1` (starts with `#` and can contain lower and upper-case letters, digits and underscores)

> If a text that should match a template is not recognized as matching, check whether there is not missing/extra whitespace at the start, end etc.

> Currently a quick-n-dirty implementation that *just works* (I hope 😅). It should definitely be automatically tested and use better practises (bindings, async operations etc.). Maybe someday...