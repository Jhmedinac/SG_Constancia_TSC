<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TimeoutControl.ascx.cs" Inherits="SG_Constancia_TSC.TimeoutControl" %>

<script type="text/javascript">
    window.SessionTimeout = (function() {
        var _timeLeft, _popupTimer, _countDownTimer;

        var stopTimers = function() {
            window.clearTimeout(_popupTimer);
            window.clearTimeout(_countDownTimer);
        };

        var updateCountDown = function() {
            var min = Math.floor(_timeLeft / 60);
            var sec = _timeLeft % 60;
            if(sec < 10)
                sec = "0" + sec;

            document.getElementById("CountDownHolder").innerHTML = min + ":" + sec;

            if(_timeLeft > 0) {
                _timeLeft--;
                _countDownTimer = window.setTimeout(updateCountDown, 1000);
            } else  {
                window.location = <%= QuotedTimeOutUrl %>;
            }            
        };

        var showPopup = function() {
            _timeLeft = 60;
            updateCountDown();
            ClientTimeoutPopup.Show();
        };

        var schedulePopup = function() {       
            stopTimers();
            _popupTimer = window.setTimeout(showPopup, <%= PopupShowDelay %>);
        };

        var sendKeepAlive = function() {
            stopTimers();
            ClientTimeoutPopup.Hide();
            ClientKeepAliveHelper.PerformCallback();
        };

        return {
            schedulePopup: schedulePopup,
            sendKeepAlive: sendKeepAlive
        };

    })();    
</script>


<dx:ASPxPopupControl ID="TimeoutPopup" runat="server" AllowDragging="True" ClientInstanceName="ClientTimeoutPopup" CloseAction="None" HeaderText="Sesión expirará" 
    Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False" ShowFooter="True" Width="250px" Font-Size="Small">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
           ¡Su sesión está a punto de caducar!
            <br />
            <br />
            <span id="CountDownHolder"></span>
            <br />
            <br />
        Haga clic en Aceptar para continuar con su sesión.
        </dx:PopupControlContentControl>
    </ContentCollection>
    <FooterTemplate>

        <dx:ASPxButton runat="server" ID="OkButton" Text="Aceptar" AutoPostBack="false">
            <ClientSideEvents Click="SessionTimeout.sendKeepAlive" />
        </dx:ASPxButton>
    </FooterTemplate>
    <FooterStyle>
        <Paddings Padding="5" />
    </FooterStyle>
</dx:ASPxPopupControl>
<dx:ASPxGlobalEvents runat="server" ID="GlobalEvents">
    <ClientSideEvents ControlsInitialized="SessionTimeout.schedulePopup" />
</dx:ASPxGlobalEvents>
<dx:ASPxCallback runat="server" ID="KeepAliveHelper" ClientInstanceName="ClientKeepAliveHelper">
</dx:ASPxCallback>
