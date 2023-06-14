namespace OverdueBookReporter.UnitTests;

public class LibraryHtmlParserTests
{
    [Fact]
    public async Task Hoi()
    {
        var html = """
            <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "DTD/xhtml1-strict.dtd">
            <html class="page cgi-event-prolreqa  wise-page wise-page-branch-1012 wise-page-instance-I010 wise-page-pubcat-false wise-page-integration-wise wise-page-has-top-navbar" default-non-bindable="">
            <head>
            <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
            <script>
                                var GactorEmail = 'true'; /* tijdelijke Globale
                                variabele*/
                                var Gquerystring = 'event=invent;var=frame;ssoid=3tsjdqpo5ni07j691as7e15gvt;ssokey=joomla;sid=f67da1ef-544a-4201-b148-0669f50e4475;prt=INTERNET;vestnr=1012;taal=nl_NL'; /* tijdelijke Globale variabele*/
                                var Gbaseurl = '/cgi-bin/bx.pl?sid=f67da1ef-544a-4201-b148-0669f50e4475;vestnr=1012;prt=INTERNET;taal=nl_NL;var=frame'; /* tijdelijke Globale variabele*/
                                var Gsid = 'f67da1ef-544a-4201-b148-0669f50e4475'; /* tijdelijke Globale variabele*/
                                var Gmag_cwise = '1'; /* tijdelijke Globale variabele*/
                                var Gw4a_profiel = '0'; /* tijdelijke Globale variabele*/
                                var Gw4a_ingelogd = '0'; /* tijdelijke Globale variabele*/
                                var Gingelogd = '1'; /* tijdelijke Globale variabele*/
                            </script><meta name="viewport" content=" user-scalable=yes">
            <meta http-equiv="X-UA-Compatible" content="IE=Edge">
            <title>Wat heb ik thuis</title>
            <link rel="icon" type="image/png" href="/images/favicon.ico">
            <link rel="stylesheet" href="/css/wise-bootstrap.css">
            <link rel="stylesheet" href="/rsrc/css/newstyle.css">
            <link rel="stylesheet" href="/wise-modules/web_icons/1/css/wise-icons.css">
            <style>
                        .wise-page.wise-page-has-top-navbar { padding-top:0; }
                        html,body { background:#fff; }
                        .wise-page-panel {padding: 0;margin-bottom: 20px;background: #fff;box-shadow: none;}
                        .modal-backdrop { background: #fff; opacity: 0.7; }
                    </style>
            <link rel="stylesheet" href="/css/wise-page.css">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <link rel="stylesheet" href="//maxcdn.bootstrapcdn.com/font-awesome/4.0.3/css/font-awesome.css">
            <link xmlns:og="http://ogp.me/ns#" rel="stylesheet" type="text/css" href="../plugins/starbox/css/starbox.css" media="screen">
            <link xmlns:og="http://ogp.me/ns#" rel="stylesheet" type="text/css" href="../plugins/modalbox/css/modalbox.css" media="screen">
            <link xmlns:og="http://ogp.me/ns#" rel="stylesheet" type="text/css" href="../plugins/autocomplete/css/autocomplete.css" media="screen">
            <link xmlns:og="http://ogp.me/ns#" rel="stylesheet" type="text/css" href="../plugins/tooltip/css/opentip.css" media="screen">
            <script type="text/javascript" src="/js/wisetranslate.js"></script><script type="text/javascript" src="/rsrc/translations/wisetxt_nl_NL.js"></script><script language="Javascript">
            function checkFields_kl() {
                var lener = document.loginform_kl.newlener.value;
                if (lener.length==0) {
                    //alert("lenernummer invullen");
                    document.loginform_kl.newlener.focus();
                    return false;
                }
                var pin   = document.loginform_kl.pinkode.value;
                if (pin.length==0) {
                    //alert("pincode invullen");
                    document.loginform_kl.pinkode.focus();
                    return false;
                }
                return true;
            }
            function doeMuziekWeb(uri,msg) {
            if (confirm(msg)) {
                doeExternWindow(uri);
            }
            }
            var FALLBACK_MS=1200000;
            function fallback() {
            if (typeof(this["_local_fallback"])=="function") {
                _local_fallback("https://wise-web.bibliotheek.rotterdam.nl/cgi-bin/bx.pl?event=clear_main&sid=f67da1ef-544a-4201-b148-0669f50e4475&vestnr=1012&prt=INTERNET&taal=nl_NL&var=frame");
            } else {    
                var url="https://wise-web.bibliotheek.rotterdam.nl/cgi-bin/bx.pl?event=clear_main&sid=f67da1ef-544a-4201-b148-0669f50e4475&vestnr=1012&prt=INTERNET&taal=nl_NL&var=frame";
                document.location = url;
            }
            }

            function doeExternWindow(uri) {
            var w = window.open(uri,"extrawindow");
            w.focus();
            }
            function Init() {
            if (typeof(this["_local_init"])=="function") {
                _local_init();
            } else {    
                setTimeout('fallback()',FALLBACK_MS);

            }
            }
            function doeHelp(uri) {
            var w = window.open(uri,"helpwindow","height=400,width=640,status=no,resizable=yes");
            w.focus();
            }
            /* hier include files */
            </script><noscript xmlns:og="http://ogp.me/ns#"><meta http-equiv="Refresh" content="0; URL=https://wise-web.bibliotheek.rotterdam.nl/cgi-bin/bx.pl?event=invent&amp;ssoid=3tsjdqpo5ni07j691as7e15gvt&amp;ssokey=joomla&amp;sid=f67da1ef-544a-4201-b148-0669f50e4475&amp;prt=INTERNET&amp;vestnr=1012&amp;taal=nl_NL&amp;var=geen_js;"></noscript>
            <script xmlns:og="http://ogp.me/ns#" type="text/javascript" src="../plugins/browserdetect/js/browserdetect.js"></script><script xmlns:og="http://ogp.me/ns#" type="text/javascript" src="/rsrc/js/prototype.js"></script><script xmlns:og="http://ogp.me/ns#" type="text/javascript" src="/rsrc/js/scriptaculous.js"></script><script xmlns:og="http://ogp.me/ns#" type="text/javascript" src="/rsrc/js/wise4all.js"></script><script xmlns:og="http://ogp.me/ns#" type="text/javascript" src="/rsrc/js/wise-common.js"></script><script xmlns:og="http://ogp.me/ns#" type="text/javascript" src="../plugins/starbox/js/starbox.js"></script><script xmlns:og="http://ogp.me/ns#" type="text/javascript" src="/rsrc/plugins/fabtabs/js/fabtabs.js"></script><script xmlns:og="http://ogp.me/ns#" type="text/javascript" src="../plugins/editinplace/js/editinplace.js"></script><script xmlns:og="http://ogp.me/ns#" type="text/javascript" src="../plugins/livevalidation/js/livevalidation_prototype.js"></script><script xmlns:og="http://ogp.me/ns#" type="text/javascript" src="../plugins/autocomplete/js/autocomplete.js"></script><script xmlns:og="http://ogp.me/ns#" type="text/javascript" src="../plugins/tooltip/js/opentip.js"></script><!--[if lte IE 9]><script xmlns:og="http://ogp.me/ns#" type="text/javascript" src="../plugins/tooltip/js/excanvas.js"></script><![endif]--><script type="text/javascript">
                            waitForProto('f67da1ef-544a-4201-b148-0669f50e4475', '0', '1012', '0',  '0',   '1',   '0',   '0', '0', 'verleng', 'https://wise-web.bibliotheek.rotterdam.nl/cgi-bin/bx.pl?event=invent&ssoid=3tsjdqpo5ni07j691as7e15gvt&ssokey=joomla&sid=f67da1ef-544a-4201-b148-0669f50e4475&prt=INTERNET&vestnr=1012&taal=nl_NL&var=geen_js');
                        </script><script type="text/javascript" src="/js/portal/carousel.js"></script><script type="text/javascript" src="/js/modernizr.min.js"></script><script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script><script src="/wise-modules/web_bootstrap/4/js/bootstrap.min.js"></script><script>
                        jQuery.noConflict();
                        if (typeof Prototype != 'undefined') {
                            if (Prototype.BrowserFeatures.ElementExtensions) {
                                var disablePrototypeJS = function (method, pluginsToDisable) {
                                            var handler = function (event) {
                                                event.target[method] = undefined;
                                                setTimeout(function () {
                                                    delete event.target[method];
                                                }, 0);
                                            };
                                            pluginsToDisable.each(function (plugin) {
                                                jQuery(window).on(method + '.bs.' + plugin, handler);
                                            });
                                        },
                                        pluginsToDisable = ['collapse', 'dropdown', 'modal', 'tooltip', 'popover', 'tab'];
                                disablePrototypeJS('show', pluginsToDisable);
                                disablePrototypeJS('hide', pluginsToDisable);
                            }
                        }
                    </script><script src="/wise-modules/web_bootstrap/4/js/wise-bootstrap.min.js" defer></script>
            </head>
            <body>
            <div id="wrapper">
            <span id="help_topmenu" style="display:none" height="0" width="0"></span><div id="container">
            <div id="content"><div id="content_middle" class="wise-page-panel">
            <div class="row">
            <div class="col-sm-3">
            <div class="padded hidden-print"><div class="padded"></div></div>
            <nav><ul class="nav nav-pills nav-stacked hidden-print">
            <li><a tabindex="0" role="button" href="https://www.bibliotheek.rotterdam.nl/wise-apps/my-account/1012/createcommunityprofile" class="btn btn-link" target="_top">Community-profiel aanmaken</a></li>
            <li class="active"><a href="#" class="" target="_self"><i class="icon-mymenu-home wise-page-icon-left" aria-hidden="true"></i>Wat heb ik thuis</a></li>
            <li class="li_menu "><a href="https://www.bibliotheek.rotterdam.nl/wise-apps/my-account/1012/holds/" class="" target="_top"><i class="icon-mymenu-hold wise-page-icon-left" aria-hidden="true"></i>Mijn reserveringen</a></li>
            <li class="li_menu "><a href="https://www.bibliotheek.rotterdam.nl/wise-apps/my-account/1012/wishlist" class="" target="_top"><i class="icon-mymenu-favorites wise-page-icon-left" aria-hidden="true"></i>Mijn verlanglijst</a></li>
            <li class="li_menu "><a href="/cgi-bin/bx.pl?event=recpers;sid=f67da1ef-544a-4201-b148-0669f50e4475;vestnr=1012;prt=INTERNET;taal=nl_NL;var=frame;aantal=10" class=""><i class="icon-mymenu-recpers wise-page-icon-left" aria-hidden="true"></i>Persoonlijk advies</a></li>
            <li class="li_menu "><a href="/cgi-bin/bx.pl?from=private;event=tickw_krtovz;sid=f67da1ef-544a-4201-b148-0669f50e4475;vestnr=1012;prt=INTERNET;taal=nl_NL;var=frame" class=""><i class="icon-mymenu-ticket wise-page-icon-left" aria-hidden="true"></i>Mijn kaarten</a></li>
            <li class="li_menu "><a href="/cgi-bin/bx.pl?dcat=0;titcode=;medium=;rubplus=;extsdef=;tref=;event=leendet;sid=f67da1ef-544a-4201-b148-0669f50e4475;vestnr=1012;prt=INTERNET;taal=nl_NL;var=frame" class=""><i class="icon-mymenu-data wise-page-icon-left" aria-hidden="true"></i>Mijn gegevens</a></li>
            <li class="li_menu "><a href="/cgi-bin/bx.pl?from=private;event=findet;sid=f67da1ef-544a-4201-b148-0669f50e4475;vestnr=1012;prt=INTERNET;taal=nl_NL;var=frame" class=""><i class="icon-mymenu-pay wise-page-icon-left" aria-hidden="true"></i>Details financiële posten</a></li>
            <li class="li_menu "><a href="/cgi-bin/bx.pl?from=private;event=finhist;sid=f67da1ef-544a-4201-b148-0669f50e4475;vestnr=1012;prt=INTERNET;taal=nl_NL;var=frame" class=""><i class="icon-mymenu-paid wise-page-icon-left" aria-hidden="true"></i>Recent betaalde posten</a></li>
            <li class="li_menu "><a href="/cgi-bin/bx.pl?from=private;event=idlstatus;sid=f67da1ef-544a-4201-b148-0669f50e4475;vestnr=1012;prt=INTERNET;taal=nl_NL;var=frame" class=""><i class="icon-mymenu-ideal wise-page-icon-left" aria-hidden="true"></i>iDEAL transacties</a></li>
            <li class="li_menu "><a href="/cgi-bin/bx.pl?dcat=0;from=private;event=inbox_list;sid=f67da1ef-544a-4201-b148-0669f50e4475;vestnr=1012;prt=INTERNET;taal=nl_NL;var=frame" class=""><i class="icon-mymenu-inbox wise-page-icon-left" aria-hidden="true"></i>Inbox</a></li>
            <li class="li_menu "><a href="https://www.bibliotheek.rotterdam.nl/wise-apps/my-account/1012/messagepreferences/" class="" target="_top"><i class="icon-mymenu-mailings wise-page-icon-left" aria-hidden="true"></i>Berichtvoorkeuren</a></li>
            <li class="li_menu "><a href="/cgi-bin/bx.pl?from=private;event=delegate_list;sid=f67da1ef-544a-4201-b148-0669f50e4475;vestnr=1012;prt=INTERNET;taal=nl_NL;var=frame" class=""><i class="icon-mymenu-relations wise-page-icon-left" aria-hidden="true"></i>Relaties</a></li>
            <li class="li_menu "><a href="/cgi-bin/bx.pl?type=D;event=leenhist;offset=1;sid=f67da1ef-544a-4201-b148-0669f50e4475;vestnr=1012;prt=INTERNET;taal=nl_NL;var=frame;aantal=10" class=""><i class="icon-mymenu-history wise-page-icon-left" aria-hidden="true"></i>Eerder geleende titels</a></li>
            <li class="li_menu "><a href="/cgi-bin/bx.pl?sid=f67da1ef-544a-4201-b148-0669f50e4475;vestnr=1012;prt=INTERNET;taal=nl_NL;var=frame;event=pasmist;" class=""><i class="icon-mymenu-lost wise-page-icon-left" aria-hidden="true"></i>Pas vermist melden</a></li>
            <li class="li_menu "><a href="mailto:klantenservice@bibliotheek.rotterdam.nl?subject=Reactie%20van%20J%20Doe%20(10420272086)%20betreffende%20:" class=""><i class="icon-mymenu-contact wise-page-icon-left" aria-hidden="true"></i>Contact</a></li>
            </ul></nav>
            </div>
            <div class="col-sm-9">
            <div class="padded-top visible-xs-block"></div>
            <h1 class="content_header" id="pageheader">Wat heb ik thuis</h1>
            <div id="messages"></div>
            <script>
                        // this function will be called on the iframe
                        var printPage = function () {
                            print();
                        }

                        // here we call the printPage in an iframe
                        var printIframe = function (id) {
                            var iframe = document.frames ? document.frames[id] : document.getElementById(id);
                            var ifWin = iframe.contentWindow || iframe;
                            iframe.focus();
                            ifWin.printPage();
                            return false;
                        }

                        // create our iframe for printing
                        var printLink = function (url) {
                            var iframe;

                            // verwijder eventueel voorgaande print-iframe
                            if (document.getElementById('print-iframe')) {
                                iframe = document.getElementById('print-iframe');
                                iframe.parentNode.removeChild(iframe);
                            }

                            // maak iframe met te printen url
                            iframe = document.createElement('iframe');

                            iframe.style.height = '0';
                            iframe.style.width = '0';
                            iframe.src = url;
                            iframe.id = 'print-iframe';
                            document.body.appendChild(iframe);

                            //prevent that modal is rendered  in IE11
                            if (navigator.userAgent.match(/Trident.*rv\:11\./)) {
                                jQuery("#print_voorkeuren").modal('hide');
                            }

                            // print iframe zodra deze geladen is
                            if (navigator.userAgent.indexOf("MSIE") > -1 && !window.opera) {
                                iframe.onreadystatechange = function () {
                                    if (iframe.readyState == "complete") {
                                        printIframe('print-iframe');
                                    }
                                };
                            } else {
                                iframe.onload = function () {
                                    printIframe('print-iframe');
                                };
                            }
                        }

                        // call printer menu
                        function printMenu() {
                            jQuery("#print_voorkeuren").modal();
                        }

                        // trigger printing list
                        function printList() {
                            var formHash = $('printlist').serialize();
                            printLink('bx.pl?' + formHash);
                        }
                    </script><div class="padded-bottom text-right hidden-print"><button class="btn btn-default" onclick="printMenu();"><i class="icon-base-print" aria-hidden="true"></i> Gehele lijst printen</button></div>
            <div id="print_voorkeuren" class="modal fade" tabindex="-1" role="dialog"><div class="modal-dialog" role="document"><div class="modal-content">
            <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button><h4 class="modal-title">Geef uw print voorkeuren op.</h4>
            </div>
            <div class="modal-body">
            <p>Bij 'Compact' worden enkel de titel en de auteur afgedrukt, </p>
            <p>bij 'Uitgebreid' worden tevens de cover en de omschrijving afgedrukt.</p>
            <form id="printlist">
            <div class="form-group">
            <label for="print-pref" class="control-label">Printwijze</label><select id="print-pref" name="action" class="form-control"><option value="print-compact">Compact</option>
            <option value="">Uitgebreid</option></select>
            </div>
            <input type="hidden" name="offset" value="1"><input type="hidden" name="aantal" value="999"><input type="hidden" name="event" value="prolreqa"><input type="hidden" name="sid" value="f67da1ef-544a-4201-b148-0669f50e4475"><input type="hidden" name="var" value="frame"><input type="hidden" name="type" value=""><br><p><input class="btn btn-primary" type="button" value="Printen" onclick="printList();return false;"></p>
            </form>
            </div>
            <div class="modal-footer"><button type="button" class="btn btn-default" data-dismiss="modal">Sluit venster</button></div>
            </div></div></div>
            <div id="results">Aantal exemplaren thuis: <span class="totaal">2</span>
            </div>
            <div class="alert alert-warning">Er kunnen geen exemplaren worden verlengd</div>
            <form id="verlengen" name="verlform" method="POST" action="/cgi-bin/bx.pl" onsubmit="doVerlengAll('verlengen');return false;" enctype="application/x-www-form-urlencoded">
            <ul class="list_titels">
            <li class="list_items 1">
            <div class="list_image"><a href="https://www.bibliotheek.rotterdam.nl/wise-apps/catalog/1012/detail/wise/1239271" target="_top"><img class="list_cover_image" id="" src="/cgi-bin/momredir.pl?size=&amp;lid=2017274665;ppn=413137759;isbn=9789030503439;key=1239271;" title="" alt=""></a></div>
            <div class="list_text full">
            <a class="title" title="Details van deze titel" href="https://www.bibliotheek.rotterdam.nl/wise-apps/catalog/1012/detail/wise/1239271" target="_top">Ik kan alleen wormen tekenen</a><div class="regel_container_actor"><strong class="discussie_subkop">Auteur:Mabbitt, Will</strong></div>
            <span class="countdown eta-days">Morgen</span>Kan niet worden verlengd. Maximum aantal (3) verlengingen bereikt.<ul><li>Geleend op: <span class="vet">04-04-2023</span> | Inleverdatum: <span class="vet">15-06-2023</span> | Uit vestiging: <span class="vet">Schiebroek</span> | <br>Exemplaar: <span class="vet">10000038450115</span>
            </li></ul>
            </div>
            <div class="list_buttons"><div class="waardering" id="1239271"></div></div>
            </li>
            <li class="list_items 1">
            <div class="list_image"><a href="https://www.bibliotheek.rotterdam.nl/wise-apps/catalog/1012/detail/wise/1220855" target="_top"><img class="list_cover_image" id="" src="/cgi-bin/momredir.pl?size=&amp;lid=2017032133;ppn=409827045;isbn=9789462912199;key=1220855;" title="" alt=""></a></div>
            <div class="list_text full">
            <a class="title" title="Details van deze titel" href="https://www.bibliotheek.rotterdam.nl/wise-apps/catalog/1012/detail/wise/1220855" target="_top">Olivier en het Brulmonster</a><div class="regel_container_actor"><strong class="discussie_subkop">Auteur:Hooft, Mieke van</strong></div>
            <span class="countdown eta-days">Morgen</span>Kan niet worden verlengd. Maximum aantal (3) verlengingen bereikt.<ul><li>Geleend op: <span class="vet">04-04-2023</span> | Inleverdatum: <span class="vet">15-06-2023</span> | Uit vestiging: <span class="vet">Schiebroek</span> | <br>Exemplaar: <span class="vet">10000038790801</span>
            </li></ul>
            </div>
            <div class="list_buttons"><div class="waardering" id="1220855"></div></div>
            </li>
            </ul>
            <input type="hidden" name="ander_id" value="" id="ander_id"><input type="hidden" name="event" value="prolacta" id="event"><input type="hidden" name="sid" value="f67da1ef-544a-4201-b148-0669f50e4475" id="sid"><input type="hidden" name="vestnr" value="1012" id="vestnr"><input type="hidden" name="prt" value="INTERNET" id="prt"><input type="hidden" name="taal" value="nl_NL" id="taal"><input type="hidden" name="var" value="frame" id="var">
            </form>
            <div id="haal_titelgegevens"></div>
            <script type="text/javascript" language="javascript" charset="utf-8">
                        var titcodes = $A();
                        $$('div.waardering').each(
                        function(elem, index) {
                        if (elem.id) {
                        titcodes[index] = elem.id;
                        }
                        }
                        );
                        if (titcodes.length > 0) {
                        var url = '/cgi-bin/bx.pl?event=w2a;action=haalTitels20Gegevens;items=' + titcodes;
                        new Ajax.Updater('haal_titelgegevens', url,
                        {
                        asynchronous:true,
                        method: 'get',
                        parameters: { sid: 'f67da1ef-544a-4201-b148-0669f50e4475', vestnr: '1012', var: 'frame'},
                        evalScripts: true
                        }
                        );
                        }
                    </script><div class="content_module"></div>
            </div>
            </div>
            <div id="nav_buttons" class="hidden-print"><ul><li><a id="terug" title="Een scherm terug" href="javascript:history.back()">Terug</a></li></ul></div>
            </div></div>
            <div class="clear"></div>
            </div>
            <div class="clear"></div>
            <div id="footer"><div id="footer_inner"></div></div>
            <script language="Javascript">
                    Init();
                    </script><div><div id="bootstrap-modal" class="modal fade" tabindex="-1" role="dialog"><div class="modal-dialog" role="document"><div class="modal-content">
            <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button><h4 class="modal-title"></h4>
            </div>
            <div class="modal-body"></div>
            <div class="modal-footer"><button type="button" class="btn btn-default" data-dismiss="modal">Sluiten</button></div>
            </div></div></div></div>
            <div><div id="login-modal" class="modal fade" tabindex="-1" role="dialog" aria-label="Aanmelden"><div class="modal-dialog" role="document"><div class="modal-content">
            <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button><h4 class="modal-title">Aanmelden</h4>
            </div>
            <div class="modal-body">
            <form class="form" id="login_kl" name="userform" method="post" action="/cgi-bin/bx.pl" onsubmit="loginLokaal('f67da1ef-544a-4201-b148-0669f50e4475','1012','event=invent;var=frame;ssoid=3tsjdqpo5ni07j691as7e15gvt;ssokey=joomla;sid=f67da1ef-544a-4201-b148-0669f50e4475;prt=INTERNET;vestnr=1012;taal=nl_NL','login_kl'); return false;">
            <div class="form-group">
            <label for="newlener" class="default-value">Gebruikersnaam of pasnummer</label><input class="form-control" type="text" placeholder="Gebruikersnaam of pasnummer" name="newlener" id="newlener" size="10" title="Gebruikersnaam of pasnummer">
            </div>
            <div class="form-group">
            <label for="pinkode" class="default-value">Wachtwoord</label><input class="form-control" type="password" placeholder="Wachtwoord" name="pinkode" id="pinkode" size="10" maxlength="40" title="Wachtwoord/pincode">
            </div>
            <div class="form-group"><button id="login_kl_submit" type="submit" value="Aanmelden" class="btn btn-primary">Aanmelden</button></div>
            <input type="hidden" name="vestnr" value="1012"><input type="hidden" name="sid" value="f67da1ef-544a-4201-b148-0669f50e4475"><input type="hidden" name="event" value="private"><p><a href="/cgi-bin/bx.pl?sid=f67da1ef-544a-4201-b148-0669f50e4475;vestnr=1012;prt=INTERNET;taal=nl_NL;var=frame;event=wwmailform">Wachtwoord kwijt?</a></p>
            </form>
            <p id="login_melding_lokaal" class="melding_leeg"></p>
            </div>
            <div class="modal-footer"><button type="button" class="btn btn-default" data-dismiss="modal">Sluiten</button></div>
            </div></div></div></div>
            </div>
            <script src="/rsrc/frame/js/jquery.ba-resize.min.js"></script><script type="text/javascript">

                        // jQuery is running in noConflict mode

                        // Get height of document
                        function getDocHeight(doc) {
                            doc = doc || document;
                            // from http://stackoverflow.com/questions/1145850/get-height-of-entire-document-with-javascript
                            var wrapper = jQuery('#wrapper')[0];
                            var height = Math.max(wrapper.offsetHeight, wrapper.scrollHeight, wrapper.clientHeight);
                            return height;
                        }

                        // send docHeight onload
                        function sendDocHeightMsg(e) {
                            var ht = getDocHeight();
                            parent.postMessage( {docHeight: ht}, '*' );
                        }

                        
                                    let msg = {
                                        onloginmodal : "J Doe",
                                        sid : "f67da1ef-544a-4201-b148-0669f50e4475"
                                    };
                                    parent.postMessage( msg, '*' );
                                

                        jQuery(document).ready(function($){
                            // onload resize iframe
                            sendDocHeightMsg();

                            // assign jquery onresize handler
                            jQuery("#container").resize(function (e) {
                                sendDocHeightMsg(e);
                            });
                        });
                    </script><div id="hide-all-modalboxes">
            <div id="login_content" class="modal fade" tabindex="-1" role="dialog"><div class="modal-dialog" role="document"><div class="modal-content">
            <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button><h4 class="modal-title">U dient zich eerst aan te melden.</h4>
            </div>
            <div class="modal-body">
            <div id="login_melding" class="melding_leeg"></div>
            <form id="login_local_form" class="login_local_form" onsubmit="loginLocal('f67da1ef-544a-4201-b148-0669f50e4475', '', '1012', '', 'login_local_form');return false;" method="post">
            <div class="form-group">
            <label for="newlener" class="default-value">Gebruikersnaam of pasnummer</label><input class="form-control login_local_lenernummer" type="text" placeholder="Gebruikersnaam of pasnummer" name="lokaal_lenernummer" id="lokaal_lenernummer" size="10" title="Gebruikersnaam of pasnummer">
            </div>
            <div class="form-group">
            <label for="pinkode" class="default-value">Wachtwoord</label><input class="form-control login_local_pincode" type="password" placeholder="Wachtwoord" name="lokaal_pincode" id="lokaal_pincode" size="10" maxlength="40" title="Wachtwoord/pincode">
            </div>
            <div class="form-group"><button id="submit" type="submit" value="Aanmelden" class="btn btn-primary">Aanmelden</button></div>
            </form>
            <p><a href="/cgi-bin/bx.pl?sid=f67da1ef-544a-4201-b148-0669f50e4475;vestnr=1012;prt=INTERNET;taal=nl_NL;var=frame;event=wwmailform">Wachtwoord kwijt?</a></p>
            </div>
            <div class="modal-footer"><button type="button" class="btn btn-default" data-dismiss="modal">Sluiten</button></div>
            </div></div></div>
            <div id="login_geen_email" class="modal fade" tabindex="-1" role="dialog"><div class="modal-dialog" role="document"><div class="modal-content">
            <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button><h4 class="modal-title">Om uw mening te kunnen geven, heeft u een Community-account nodig. Daarvoor moet uw e-mailadres bij ons bekend zijn. Geef uw e-mailadres op via </h4>
            </div>
            <div class="modal-body"><div id="login_geen_email_melding" class="alert alert-danger">Om uw mening te kunnen geven, heeft u een Community-account nodig. Daarvoor moet uw e-mailadres bij ons bekend zijn. Geef uw e-mailadres op via <a href="/cgi-bin/bx.pl?sid=f67da1ef-544a-4201-b148-0669f50e4475;vestnr=1012;prt=INTERNET;taal=nl_NL;var=frame;event=leenmut;" class="alert-link">Algemene lenergegevens</a>
            </div></div>
            <div class="modal-footer"><button type="button" class="btn btn-default" data-dismiss="modal">Sluiten</button></div>
            </div></div></div>
            <div id="login_algemene_voorwaarden" class="modal fade" tabindex="-1" role="dialog"><div class="modal-dialog" role="document"><div class="modal-content">
            <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button><h4 class="modal-title">Algemene Voorwaarden Wise Community</h4>
            </div>
            <div class="modal-body">
            <div id="algemene_voorwaarden_attendering"><div class="alert alert-info"><strong id="algemene_voorwaarden_omschrijving">Om een Community-profiel aan te maken moet u eerst akkoord gaan met de Algemene Voorwaarden.</strong></div></div>
            <div class="breaker"></div>
            <h3>OCLC, Inc. Algemene Voorwaarden Wise Community</h3>
            <div class="voorwaarden"><div class="container-fluid">
            <div class="row"><div clas="col-xs-12">N.B. Deze voorwaarden zijn aanvullend op de voorwaarden die de bibliotheek hanteert en waarmee je reeds akkoord bent gegaan. Hiermee accepteer je de algemene voorwaarden van OCLC BV voor het gebruik van Wise Community.<br/><br/>De onderstaande algemene voorwaarden (de "Overeenkomst") zijn van toepassing op je gebruik van de Wise Community-service (de "Wise Community" of “Site”), aangeboden door OCLC, Inc. ("OCLC"). Door de Site te gebruiken of te bezoeken, ga je er uitdrukkelijk mee akkoord om gebonden te zijn aan de voorwaarden zoals in deze Overeenkomst uiteen zijn gezet alsmede de op de Wise Community en de Site toepasselijke wet- en regelgeving. Als je het niet eens bent met de voorwaarden van deze overeenkomst, dien je de site en/of de componenten daarvan niet te gebruiken en je account te verwijderen.<h4>
            <br>1. Correct gebruik van de Wise Community</h4>
            <p>Tenzij anders vermeld, is de inhoud van deze site eigendom van OCLC en/of zijn leveranciers of licentiegevers, en dergelijke inhoud en software wordt beschermd door Amerikaanse en internationale intellectuele eigendomswetten. Dienovereenkomstig stem je ermee in dat je geen inhoud van de Site zult kopiëren, reproduceren, wijzigen, afgeleide werken zult maken of openbaar zult weergeven (behalve voor je eigen niet-commercieel gebruik), behalve in overeenstemming met de voorwaarden van deze Overeenkomst. Je gaat er ook mee akkoord dat je geen enkele robot, spin, ander geautomatiseerd apparaat of handmatig proces zult gebruiken om inhoud van de site te controleren of te kopiëren, tenzij uitdrukkelijk anders overeengekomen.<br/>Bovendien wordt de Site aan je verstrekt voor eigen gebruik en mag deze niet voor commerciële doeleinden worden gebruikt, behalve in overeenstemming met de voorwaarden van deze Overeenkomst. Je mag bijvoorbeeld geen van de volgende acties uitvoeren zonder schriftelijke toestemming van OCLC: de Site gebruiken om een product of dienst te verkopen of aan te bieden; de Site gebruiken om het verkeer naar je website te vergroten om commerciële redenen, zoals advertentieverkoop; de resultaten van de Site kopiëren, herformatteren en weergeven, of een kopie van enig deel van de Site weergeven op een andere website. Elk gebruik van de Site dat inbreuk maakt op de intellectuele eigendomsrechten van OCLC en/of haar leveranciers of licentiegevers, of dat voor commerciële doeleinden is, zal worden onderzocht en OCLC heeft het recht om passende civielrechtelijke en strafrechtelijke stappen te ondernemen. </p>
            </div></div>
            <div class="row"><div clas="col-xs-12">
            <h4>
            <br>2. Wise Community-componenten, gebruikersnaam</h4>De componenten van Wise Community omvatten het volgende: </div></div>
            <div class="row">
            <div class="col-xs-1"></div>
            <div class="col-xs-11">
            <br><ul style="list-style: disc;">
            <li>Recensies (recensies door gebruikers)</li>
            <li>Beoordeling (beoordelingen door gebruikers)</li>
            <li>Tags (sociale tags met eigen zoekwoorden door gebruikers)</li>
            <li>Lijsten (titellijsten van gebruikers)</li>
            <li>Vrienden (vrienden worden met andere bibliotheekleden in het land)</li>
            <li>Suggesties aan een vriend (stuur een titeltip naar vrienden)</li>
            </ul>
            </div>
            </div>
            <div class="row"><div clas="col-xs-12">Bij het aanmaken van je profiel word je gevraagd een gebruikersnaam te kiezen. Dit is de naam die wordt weergegeven bij je bijdragen in de Wise Community. Deze gebruikersnaam moet uniek zijn binnen de Wise Community (het systeem zorgt hiervoor) en mag niet aanstootgevend zijn voor anderen en mag geen obscene taal bevatten. Je mag je niet voordoen als een andere persoon of instelling. Je gebruikersnaam mag ook niet ten onrechte de indruk wekken dat je OCLC, de Wise Community of een aangesloten bibliotheek op enigerlei wijze vertegenwoordigt. Op je profielpagina kun je aangeven in hoeverre jouw gegevens getoond mogen worden aan anderen. Jouw persoonlijk identificeerbare informatie wordt behandeld in overeenstemming met het OCLC Privacy-overzicht, te vinden op <a href="https://policies.oclc.org/en/privacy/privacy-statement.html" target="_blank">https://policies.oclc.org/en/privacy/privacy-statement.html</a>. </div></div>
            <div class="row"><div clas="col-xs-12">
            <h4>
            <br>3. Melden van misbruik</h4>Als je denkt dat een inzending aan de community deze voorwaarden schendt, kun je de knop ‘Meld misbruik' gebruiken om dit te melden. Wanneer een beoordeling door voldoende mensen wordt gerapporteerd, wordt deze verwijderd (de persoon in kwestie wordt hiervan niet in kennis gesteld). De knop 'Meld misbruik' is niet bedoeld om te laten zien dat je het niet eens bent met de inhoud van een recensie. Het betreft dan een meningsverschil, waarvoor de Site een knop 'Niet nuttig' biedt.</div></div>
            <div class="row"><div clas="col-xs-12">
            <h4>
            <br>4. Bijdrage van de gebruiker en privacy, schending van voorwaarden</h4>Je mag alleen inhoud toevoegen aan de Wise Community waarvoor je de bijbehorende intellectuele eigendomsrechten bezit; het is je verboden inhoud te uploaden die inbreuk maakt op de intellectuele eigendomsrechten van anderen. Indien je inbreukmakende inhoud uploadt, ga je ermee akkoord om samen te werken met OCLC om de kwestie op eigen kosten op te lossen.<br/><br/>Wanneer je inhoud toevoegt aan de Wise Community, tenzij je anders aangeeft, verleen je OCLC een niet-exclusief, royaltyvrij, eeuwigdurend, onherroepelijk en volledig sublicentieerbaar recht om te gebruiken, reproduceren, wijzigen, aanpassen, publiceren, uitvoeren, vertalen, afgeleide werken te maken van, verspreiden en tonen van dergelijke inhoud over de hele wereld in alle media.<br/><br/>Bijdragen geleverd aan de Wise Community worden aangeboden zonder verwachting van compensatie, partnerschap of erkenning buiten de associatie van de opmerking met de auteur in het forum waarin het wordt aangeboden.<br/><br/>Wise Community bewaart je inloggegevens (naam, adres en klantnummer), die uitsluitend worden gebruikt om Wise Community diensten te verlenen. OCLC deelt deze gegevens niet met derde partijen. OCLC behoudt zich echter het recht voor om bepaalde geaggregeerde gegevens te gebruiken en/of aan derden te verstrekken. Raadpleeg het OCLC Privacy-overzicht voor meer informatie.<br/><br/>Je account is gekoppeld aan je persoonlijke bibliotheekabonnement. Deel je accountgegevens niet met derden die niet het recht hebben om dit abonnement te gebruiken. Het is je niet toegestaan inhoud te plaatsen die illegaal, obsceen, bedreigend of lasterlijk is, de rechten van anderen schendt (inclusief intellectuele eigendomsrechten en privacyrechten), anderszins beledigend of aanstootgevend is of een politieke of commerciële boodschap bevat, zoals bepaald door OCLC in haar eigen oordeel.<br/><br/>Naast alle juridische rechtsmiddelen die OCLC kan hebben voor een schending door je van de voorwaarden van deze Overeenkomst, hebben OCLC en / of je bibliotheek ook het recht om naar eigen oordeel je toegang tot de Site op te schorten of te beëindigen.</div></div>
            <div class="row"><div clas="col-xs-12">
            <h4>
            <br>5. Verantwoordelijkheid en continuïteit van Wise Community, geen garantie</h4>OCLC geeft geen garantie dat de diensten die als onderdeel van deze Site worden geleverd, betrouwbaar, nauwkeurig, volledig, up-to-date of anderszins geldig zijn. De informatie die gebruikers op Wise Community plaatsen, wordt niet geverifieerd door OCLC en OCLC is niet verantwoordelijk voor mogelijke fouten of tekortkomingen in de verstrekte informatie en er kunnen geen rechten worden ontleend aan de inhoud van de Site. Daarom wordt de site geleverd "zoals het is" zonder enige vorm van garantie en gebruik je de Site op eigen risico. OCLC wijst uitdrukkelijk elke garantie af, uitdrukkelijke of impliciete, betreffende de website of de inhoud ervan, inclusief enige impliciete garantie van verkoopbaarheid, geschiktheid voor een bepaald doel of het niet inbreuk maken op enig recht.</div></div>
            <div class="row"><div clas="col-xs-12">
            <h4>
            <br>6. Beperking van aansprakelijkheid, onschadelijk houden en vrijwaring</h4>OCLC is in geen geval aansprakelijk voor enige indirecte, incidentele, gevolgschade, speciale of voorbeeldschade die voortvloeit uit of in verband met het gebruik van de Site, ook niet indien OCLC is geadviseerd van de mogelijkheid van dergelijke schade. OCLC is in geen geval aansprakelijk voor enige schade.<br/><br/>Je stemt ermee in OCLC en zijn werknemers, agenten en vertegenwoordigers te vrijwaren tegen claims van derden die voortvloeien uit of op enigerlei wijze verband houden met je gebruik van de Site, inclusief aansprakelijkheid of kosten die voortvloeien uit alle claims, verliezen, schade (feitelijk en vervolg), rechtszaken, vonnissen, proceskosten en advocatenkosten, van welke aard dan ook. In dat geval zal OCLC je schriftelijk op de hoogte stellen van een dergelijke claim, rechtszaak of actie.</div></div>
            <div class="row"><div clas="col-xs-12">
            <h4>
            <br>7. Diversen</h4>Je mag deze Overeenkomst of daaruit voortvloeiende rechten of verplichtingen niet overdragen, geheel of gedeeltelijk, vrijwillig of van rechtswege, zonder voorafgaande schriftelijke toestemming van OCLC. Een dergelijke vermeende overdracht door jou zonder voorafgaande schriftelijke toestemming van OCLC is nietig en zonder kracht of gevolg, tenzij OCLC uitdrukkelijk anderszins naar eigen oordeel instemt. Niettegenstaande enige bepaling van deze Overeenkomst, zal voor alle doeleinden van deze Overeenkomst elke partij optreden als een onafhankelijke contractant en niet als partner, joint venture-deelnemer, agent, werknemer of werkgever van de ander en zal de ander niet binden aan of proberen te binden aan een contract. Als een bepaling van deze Overeenkomst ongeldig of niet-afdwingbaar wordt geacht, wordt die bepaling geacht te zijn vervangen door een geldige afdwingbare bepaling die het meest overeenkomt met de bedoeling van de oorspronkelijke bepaling en worden de overige bepalingen gehandhaafd. Het nalaten van OCLC om op te treden met betrekking tot een inbreuk door jou of anderen, doet geen afstand van het recht van OCLC om op te treden met betrekking tot volgende of soortgelijke inbreuken. Indien OCLC enig recht of bepaling van deze voorwaarden niet uitoefent of afdwingt, betekent dit niet dat afstand wordt gedaan van een dergelijk recht of deze bepaling. De sectie-koppen en sub-koppen in deze Overeenkomst zijn alleen opgenomen voor het gemak en zijn niet beperkt of anderszins van invloed op de voorwaarden van deze Overeenkomst. Op deze overeenkomst is Nederlands recht van toepassing. OCLC heeft het recht om de voorwaarden van deze Overeenkomst op elk moment te wijzigen, welke wijziging onmiddellijk van kracht wordt na plaatsing op de Site. Deze overeenkomst vormt de volledige overeenkomst tussen OCLC en jou met betrekking tot het onderwerp hiervan.</div></div>
            </div></div>
            <br><form id="algemene_voorwaarden_form" onsubmit="akkoordAlgemeneVoorwaarden('f67da1ef-544a-4201-b148-0669f50e4475');return false">
            <div id="algemenevoorwaarden_melding" class="melding_leeg"></div>
            <input id="algemene_voorwaarden_check" type="checkbox" class="algemene_voorwaarden_check" name="algemene_voorwaarden_check"><label for="algemene_voorwaarden_check"><strong> Ik heb de Algemene Voorwaarden gelezen en ik ga akkoord.</strong></label><br><input id="algemene_voorwaarden_akkoord" value="Akkoord" class="algemene_voorwaarden_akkoord btn btn-primary" type="submit" name="algemene_voorwaarden_akkoord">
            </form>
            </div>
            <div class="modal-footer"><button type="button" class="btn btn-default" data-dismiss="modal">Sluiten</button></div>
            </div></div></div>
            <div id="login_mag_cwise" class="modal fade" tabindex="-1" role="dialog"><div class="modal-dialog" role="document"><div class="modal-content">
            <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button><h4 class="modal-title">Voor deze inschrijving is deelname aan Wise Community niet toegestaan.</h4>
            </div>
            <div class="modal-body"><div class="alert alert-danger">Voor deze inschrijving is deelname aan Wise Community niet toegestaan.</div></div>
            <div class="modal-footer"><button type="button" class="btn btn-default" data-dismiss="modal">Sluiten</button></div>
            </div></div></div>
            <div id="login_nieuwprofiel" class="modal fade" tabindex="-1" role="dialog"><div class="modal-dialog" role="document"><div class="modal-content">
            <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button><h4 class="modal-title">Gewenste nickname: </h4>
            </div>
            <div class="modal-body">
            <div class="alert alert-info">Geef hier de nickname op die u wilt gebruiken voor recensies, tags etc. Uw nickname moet bestaan uit minimaal 4 letters en/of cijfers. Als een nickname al in gebruik is, krijgt u voorbeelden van mogelijke andere namen te zien. Natuurlijk mag u ook een hele andere nickname kiezen.</div>
            <div class="alert alert-info">Tip: Uw nickname wordt op het scherm getoond. Gebruik dus niet uw pincode of postcode.</div>
            <div id="nieuwprofiel_melding" class="melding_leeg"></div>
            <form id="nieuwprofiel_form" class="nickname_form" method="post" onsubmit="loginCheckNickname('f67da1ef-544a-4201-b148-0669f50e4475', '', '', 'nieuwprofiel_form', '');return false">
            <p><label for="nickname_veld">Gewenste nickname: </label><br><div id="nickname_div"><input type="text" class="nickname" id="nickname_veld" name="nickname"></div><input class="checknicknameloader" id="checknicknameloader" style="display: none;"><br><div id="nickname_preview"></div></p>
            <br><p><input id="nieuwprofiel_form_submit" type="submit" value="Aanvragen" class="btn btn-primary"></p>
            </form>
            </div>
            <div class="modal-footer"><button type="button" class="btn btn-default" data-dismiss="modal">Sluiten</button></div>
            </div></div></div>
            <div id="login_succes" class="modal fade" tabindex="-1" role="dialog"><div class="modal-dialog" role="document"><div class="modal-content">
            <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button><h4 class="modal-title">Aanmelden</h4>
            </div>
            <div class="modal-body">
            <p><div id="login_succes_melding" class="melding_leeg"></div></p>
            <br><br><div id="nieuwprofiel_privacy_waarschuwing" style="display:none;">
            <div class="alert alert-info">Ter bescherming als gebruiker van Wise Community kunt u onder instellingen (Privacy instellingen) aangeven of u betreffende zaken zichtbaar voor derden wilt maken, of juist niet. <br><a class="bericht_knop instellingen alert-link" tabindex="0" role="button" onclick="haalProfielPrivate('f67da1ef-544a-4201-b148-0669f50e4475', '1012', '0');return false"></a>
            </div>
            <div class="alert alert-info">Aanbevolen wordt om deze instellingen na te lopen. Mocht u direct willen beginnen klik dan op <a class="alert-link" tabindex="0" role="button" onclick="jQuery('#login_succes').modal('hide');return false;" title="Sluit venster">Sluit venster</a>
                                        .

                                        </div>
            </div>
            </div>
            <div class="modal-footer"><button type="button" class="btn btn-default" data-dismiss="modal">Sluiten</button></div>
            </div></div></div>
            <div id="wijzignickname_succes" class="modal fade" tabindex="-1" role="dialog"><div class="modal-dialog" role="document"><div class="modal-content">
            <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button><h4 class="modal-title">Community nickname wijzigen</h4>
            </div>
            <div class="modal-body"><div id="wijzignickname_succes_melding" class="melding_leeg"></div></div>
            <div class="modal-footer"><button type="button" class="btn btn-default" data-dismiss="modal">Sluiten</button></div>
            </div></div></div>
            </div>
            <script type="text/javascript" src="/wise-vendor/angular-17x-combined/angular-17x.combined.min.js"></script><script type="text/javascript" src="/wise-vendor/angular-ui-bootstrap-25x/ui-bootstrap.min.js"></script><script type="text/javascript" src="/wise-apps/catalog/js/app.js"></script><script>
                        angular.element(document).ready(function(){angular.bootstrap("#wise-searchbar",["wiseSearch"],{strictDi:!0})}); // bootstrap app on searchbar area
                        angular.module("wiseSearch").run(["wiseTranslationLabelService","wiseTranslationXlatService",
                        function(wiseTranslationLabelService, wiseTranslationXlatService) {
                            wiseTranslationLabelService.setTranslationContext('I010', true);// translation context needs to be set manually since we don't use a wisePage state
                            wiseTranslationLabelService.initialLanguageReady().then(function(){
                            window.checkUnsupportedBrowser(wiseTranslationXlatService.xlatFilter('WISE-PAGE__UNSUPPORTED_BROWSER__HEADER'), wiseTranslationXlatService.xlatFilter('WISE-PAGE__UNSUPPORTED_BROWSER__SUBTITLE'));
                            });
                        }]).config(["$wiseTranslationProvider", function ($wiseTranslationProvider) {
                            $wiseTranslationProvider.setApplicationRoot("/wise-apps/catalog/"); // where can the module get its language files
                            $wiseTranslationProvider.setApplicationName("catalog"); // where can the module get its language files
                        }])
                    </script><script>
                                (function(){
                                    
                                        var locale = "nl_NL".split('_').join('-').toLowerCase();
                                        locale = JSON.stringify(locale);
                                        localStorage.setItem("wiseTranslation_activeLanguage", locale);
                                    
                                }());
                            </script><script type="text/javascript" src="/wise-modules/web_authentication/1/wise-auth-core.js"></script><script>
                        var wiseAuthCore = new WiseAuthCore();
                        
                                WiseAuthCore().initiateLogin('eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOiIxNjg2NzcxOTE1IiwiaWF0IjoiMTY4Njc2OTUxNSIsInZlcnNpb24iOiIiLCJhY3RvcklkIjoiMjk2NjQxIiwiYWN0b3JVdWlkIjoiMDQ1ZTk0ZjU3ZWRkMWY3MjMzYzRlZDMwNzAzY2MyZGEiLCJzZXNzaW9uSWQiOiJmNjdkYTFlZi01NDRhLTQyMDEtYjE0OC0wNjY5ZjUwZTQ0NzUiLCJ2ZXN0aWdpbmdJZCI6IjEwMTIiLCJicmFuY2hJZCI6IjEwMTIiLCJpbnN0YW50aWVJZCI6IkkwMTAiLCJsaWJyYXJ5SWQiOiJJMDEwIiwia2luZCI6IkkiLCJhY3Rvck5hYW0iOiJJIFMuIEJyb2JiZWwiLCJhY3Rvck5hbWUiOiJJIFMuIEJyb2JiZWwiLCJjaG9zZW5XaXNlUm9sZSI6IiIsInNlY3RvciI6IkJJRUIiLCJhdXRob3JpdGllcyI6Ik1BX0NIQU5HRV9BRERSRVNTLE1BX0NIQU5HRV9NRVNTQUdFX0RFTElWRVJZX1BSRUZFUkVOQ0VTLE1BX1NVQlNDUklCRV9DV0lTRSxNQV9SRVFVRVNUX0VNQUlMX0FERFJFU1MsTUFfQ0hBTkdFX0VNQUlMX0FERFJFU1MsTUFfQ0hBTkdFX0JSQU5DSCxNQV9WSUVXX0ZJTkFOQ0lBTF9EQVRBLE1BX1BBWU1FTlRfSURFQUwsTUFfUExBQ0VfSUJMX1JFUVVFU1QsTUFfVElDS0VUU19CT1VHSFRfT1ZFUlZJRVcsTUFfQ0hBTkdFX1RJVExFX0JPUlJPV0VEX0JFRk9SRV9OT1RJRklDQVRJT04sTUFfQUxMT1dfQkxPQ0tfQk9SUk9XSU5HX0hJU1RPUlksTUFfVklFV19CT1JST1dJTkdfSElTVE9SWSxNQV9SRVBPUlRfUEFUUk9OQ0FSRF9MT1NTLE1BX1BFUlNPTkFMX1RJVExFX1JFQ09NTUVOREFUSU9OUyxNQV9DSEFOR0VfUEFTU1dPUkQsTUFfU1VCU0NSSUJFX05FV19USVRMRVNfTk9USUZJQ0FUSU9OLE1BX1BMQUNFX0hPTERTLE1BX0NBTkNFTF9IT0xEUyxNQV9SRU5FVyxNQV9ERUxFR0FURV9SRU5FVyxNQV9ERUxFR0FURV9QQVksTUFfVVNFX1BVQkxJQ19QUklOVEVSLE1BX1ZJRVdfTE9BTlMsTUFfVklFV19QQVRST05fSU5GT1JNQVRJT04sTUFfVklFV19JTkJPWCxNQV9WSUVXX0hPTERTLE1BX1ZJRVdfV0lTSExJU1QifQ.FjSfkjDcvecgRSCE9glpAH5uTuQV2uWhZNbgQpk0uFQ');
                            </script><script></script><script type="text/javascript" src="/js/browser-tab-sync-service.js"></script><script type="text/javascript">

                        var wisePageChannel = new lib.BrowserTabSyncService();

                        wisePageChannel.startSync('/cgi-bin/bx.pl?sid=f67da1ef-544a-4201-b148-0669f50e4475;vestnr=1012;prt=INTERNET;taal=nl_NL;var=frame', 1, 1, 0 );

                    </script><script language="Javascript">
                    Init();
                    </script><script>
                        wiseAuthCore.registerApiKey({
                        apiKeyId: 'c66e838f-6fa9-4838-99c7-211a7ff42c6e',
                        apiKey: '48946a5e-da47-499a-926d-6c16fd5db0e9',
                        applicationName: 'Catalog'
                        });
                    </script><script type="text/javascript">
                        jQuery(function(){
                            let $_showPinUpdate = jQuery('#showPinUpdate');
                            if ($_showPinUpdate.length > 0) {
                                const url = '/restapi/patron/' + wiseAuthCore.getPatronSystemId() + '/library/' + wiseAuthCore.getPatronLibraryId() + '/patroninformation';
                                jQuery.get(url,function(data,status){
                                    if(data.showResetPinEditLink){
                                        $_showPinUpdate.show();
                                    } else {
                                        $_showPinUpdate.remove();
                                    }
                                });
                            }
                        });
                    </script><script type="text/javascript">
                        jQuery(function(){
                        let $_showAdditionalNameUpdate = jQuery('#showAdditionalNameUpdate');
                        if ($_showAdditionalNameUpdate.length > 0) {
                            const url = '/restapi/configuration/public/library/' + wiseAuthCore.getPatronLibraryId() + '/systemoption';
                            jQuery.get(url,function(data,status){
                                if(data.useAdditionalName){
                                    $_showAdditionalNameUpdate.show();
                                } else {
                                    $_showAdditionalNameUpdate.remove();
                                }
                            });
                        }
                    });
                    </script>
            </body>
            </html>
        """;

        var booksListing = await LibraryHtmlParser.ParseBookListingAsync(html);
        booksListing.Should().NotBeEmpty();
        booksListing.Should().HaveCount(2);

        var firstLendOutBook = booksListing.First();
        firstLendOutBook.Name.Should().Be("Ik kan alleen wormen tekenen");

        booksListing.Last().Name.Should().Be("Olivier en het Brulmonster");

    }
}