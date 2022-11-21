package application;

import java.net.URL;
import java.util.ResourceBundle;

import javafx.fxml.FXML;
import javafx.fxml.Initializable;
import javafx.scene.control.Button;
import javafx.scene.control.Label;
import javafx.event.EventHandler;
import javafx.event.ActionEvent;

public class LoggedInController implements Initializable{
	
	@FXML
	private Button button_logout;
	
	@FXML
	private Label label_welcome;
	
	@Override
	public void initialize(URL arg0, ResourceBundle resources) {
		
		button_logout.setOnAction(new EventHandler<ActionEvent>() {
			@Override
			public void handle(ActionEvent event) {
				DBUtils.changeScene(event,  "sample.fxml", "Log in!", null);
			}
		});
	}
	public void setUserInformation(String username) {
		label_welcome.setText("Welcome "+ username);
	}
}
