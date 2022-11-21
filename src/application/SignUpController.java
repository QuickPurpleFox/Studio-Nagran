package application;

import java.net.URL;
import java.util.ResourceBundle;

import javafx.event.ActionEvent;
import javafx.event.EventHandler;
import javafx.fxml.FXML;
import javafx.fxml.Initializable;
import javafx.scene.control.Alert;
import javafx.scene.control.Button;
import javafx.scene.control.TextField;

public class SignUpController implements Initializable{

	@FXML 
	private Button button_signup;
		
	@FXML 
	private Button button_log_in;
	
	@FXML
	private TextField tf_reg_username;
	
	@FXML
	private TextField tf_reg_password;
	
	@Override
	public void initialize(URL arg0, ResourceBundle arg1) {
		
		button_signup.setOnAction(new EventHandler<ActionEvent>() {

			@Override
			public void handle(ActionEvent event) {
				if(!tf_reg_username.getText().trim().isEmpty() && !tf_reg_password.getText().trim().isEmpty()) {
					DBUtils.signUpUser(event, tf_reg_username.getText(), tf_reg_password.getText());
				}else {
					System.out.println("Fill all!");
					Alert alert = new Alert(Alert.AlertType.ERROR);
					alert.setContentText("Please fill in all information to sign up!");
					alert.show();
				}
				
			}
			
		});
		
		button_log_in.setOnAction(new EventHandler<ActionEvent>() {

			@Override
			public void handle(ActionEvent event) {
				DBUtils.changeScene(event, "sample.fxml", "Log in!", null);
				
			}
		});
	}

}
