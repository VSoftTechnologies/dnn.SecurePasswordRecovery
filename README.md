# Secure Password Recovery for DNN
IowaComputerGurus's Secure Password Recovery module is the next step in helping keep your users information secure within your DotNetNuke Portal.  Unlike the out of the box functionality of DotNetNuke, this module allows users to request a password reset.  The user is then sent an "Access Code", using this code they can return to the site and reset their password.  It is only at that time that the users password was actually changed.

Using this process provides a number of security benefits for users and portal administrators.

*Users passwords are never sent via e-mail using this process
*Users passwords are only changed after the user takes action from the e-mail.  By following this approach malicious attempts will not lock users out of their accounts.
*Users that forgot their username can still request resets via e-mail.

##Minimum DNN Versions
Latest version requires DNN 8.0.0 and later.
